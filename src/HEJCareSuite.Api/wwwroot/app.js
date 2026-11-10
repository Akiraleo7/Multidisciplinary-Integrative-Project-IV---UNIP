const loginScreen = document.querySelector("#login-screen");
const appShell = document.querySelector("#app-shell");
const loginForm = document.querySelector("#login-form");
const loginStatus = document.querySelector("#login-status");
const logoutButton = document.querySelector("#logout-button");
const userName = document.querySelector("#user-name");
const userRole = document.querySelector("#user-role");
const userAvatar = document.querySelector("#user-avatar");
const pageUserName = document.querySelector("#page-user-name");
const pageTitle = document.querySelector("#page-title");
const breadcrumbLabel = document.querySelector("#breadcrumb-label");
const topbarDate = document.querySelector("#topbar-date");
const mobileMenuButton = document.querySelector("#mobile-menu-button");
const sidebar = document.querySelector(".sidebar");
const patientCount = document.querySelector("#patient-count");
const patientListCount = document.querySelector("#patient-list-count");
const form = document.querySelector("#patient-form");
const formStatus = document.querySelector("#form-status");
const listStatus = document.querySelector("#list-status");
const patientList = document.querySelector("#patient-list");
const formTitle = document.querySelector("#form-title");
const submitButton = document.querySelector("#submit-button");
const cancelButton = document.querySelector("#cancel-button");
const triageForm = document.querySelector("#triage-form");
const triagePatient = document.querySelector("#triage-patient");
const triageStatus = document.querySelector("#triage-status");
const triageListStatus = document.querySelector("#triage-list-status");
const triageList = document.querySelector("#triage-list");
const admissionForm = document.querySelector("#admission-form");
const admissionPatient = document.querySelector("#admission-patient");
const admissionStatus = document.querySelector("#admission-status");
const dischargeForm = document.querySelector("#discharge-form");
const admissionListStatus = document.querySelector("#admission-list-status");
const admissionList = document.querySelector("#admission-list");
const cancelDischargeButton = document.querySelector("#cancel-discharge-button");
let editingPatientId = null;
let selectedTriagePatient = null;
let selectedAdmissionPatient = null;
let selectedAdmissionId = null;
let currentUser = null;
let patients = [];

const viewLabels = {
  "dashboard-view": "Overview",
  "patients-view": "Patients",
  "triage-view": "Emergency triage",
  "admissions-view": "Beds & admissions"
};

function showView(viewId) {
  document.querySelectorAll(".page-view").forEach((view) => {
    view.classList.toggle("hidden", view.id !== viewId);
  });
  document.querySelectorAll(".nav-item").forEach((item) => {
    item.classList.toggle("active", item.dataset.view === viewId);
  });
  const label = viewLabels[viewId] || "Overview";
  breadcrumbLabel.textContent = label;
  pageTitle.innerHTML = viewId === "dashboard-view"
    ? `Good morning, <span id="page-user-name">${escapeHtml(currentUser?.role || "Administrator")}</span>`
    : label;
  sidebar.classList.remove("open");
  if (viewId === "patients-view") loadPatients();
}

function openWorkspace(user) {
  currentUser = user;
  const displayName = user.role || user.email || "Staff member";
  userName.textContent = displayName;
  userRole.textContent = user.email || "Administrator";
  pageUserName.textContent = displayName;
  userAvatar.textContent = displayName.slice(0, 1).toUpperCase();
  loginScreen.classList.add("hidden");
  appShell.classList.remove("hidden");
  showView("dashboard-view");
  loadPatients();
}

function closeWorkspace() {
  currentUser = null;
  patients = [];
  appShell.classList.add("hidden");
  loginScreen.classList.remove("hidden");
  loginForm.reset();
  loginStatus.textContent = "Signed out.";
  patientList.replaceChildren();
  clearTriage();
  clearAdmission();
  endEdit();
}

async function loadPatients() {
  listStatus.textContent = "Loading patient records...";
  listStatus.classList.remove("error");
  try {
    const response = await fetch("/api/v1/patients");
    if (!response.ok) throw await readApiError(response, "Could not load patients.");
    patients = await response.json();
    patientCount.textContent = patients.length;
    patientListCount.textContent = `${patients.length} record${patients.length === 1 ? "" : "s"}`;
    patientList.replaceChildren();
    patients.forEach(renderPatient);
    listStatus.textContent = patients.length ? "" : "No patients registered yet.";
  } catch (error) {
    listStatus.textContent = error.message;
    listStatus.classList.add("error");
  }
}

function renderPatient(patient) {
  const item = document.createElement("li");
  item.innerHTML = `<strong>${escapeHtml(patient.fullName)}</strong>
    <span>${patient.birthDate} · ${escapeHtml(patient.documentNumber)}</span>
    <span class="patient-actions">
      <button class="edit-button" type="button">Edit</button>
      <button class="delete-button" type="button">Delete</button>
      <button class="triage-button" type="button">Triage</button>
      <button class="admission-button" type="button">Admission</button>
    </span>`;
  item.querySelector(".edit-button").addEventListener("click", () => beginEdit(patient));
  item.querySelector(".delete-button").addEventListener("click", () => deletePatient(patient));
  item.querySelector(".triage-button").addEventListener("click", () => selectTriagePatient(patient));
  item.querySelector(".admission-button").addEventListener("click", () => selectAdmissionPatient(patient));
  patientList.append(item);
}

form.addEventListener("submit", async (event) => {
  event.preventDefault();
  formStatus.textContent = "Saving patient...";
  formStatus.classList.remove("error");
  const data = Object.fromEntries(new FormData(form));
  try {
    const response = await fetch(
      editingPatientId ? `/api/v1/patients/${editingPatientId}` : "/api/v1/patients",
      {
        method: editingPatientId ? "PUT" : "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
      });
    if (!response.ok) throw await readApiError(response, "Could not save patient.");
    form.reset();
    formStatus.textContent = editingPatientId ? "Patient updated." : "Patient registered.";
    endEdit();
    await loadPatients();
  } catch (error) {
    formStatus.textContent = error.message;
    formStatus.classList.add("error");
  }
});

triageForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  if (!selectedTriagePatient) {
    triageStatus.textContent = "Choose a patient from the Patients screen first.";
    triageStatus.classList.add("error");
    return;
  }
  triageStatus.textContent = "Saving triage...";
  triageStatus.classList.remove("error");
  const data = Object.fromEntries(new FormData(triageForm));
  data.riskLevel = Number(data.riskLevel);
  try {
    const response = await fetch(`/api/v1/patients/${selectedTriagePatient.id}/triage`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data)
    });
    if (!response.ok) throw await readApiError(response, "Could not register triage.");
    triageForm.reset();
    triageForm.elements.riskLevel.value = "3";
    triageStatus.textContent = "Triage registered.";
    await loadTriage(selectedTriagePatient);
  } catch (error) {
    triageStatus.textContent = error.message;
    triageStatus.classList.add("error");
  }
});

admissionForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  if (!selectedAdmissionPatient) {
    admissionStatus.textContent = "Choose a patient from the Patients screen first.";
    admissionStatus.classList.add("error");
    return;
  }
  admissionStatus.textContent = "Saving admission...";
  admissionStatus.classList.remove("error");
  try {
    const response = await fetch(`/api/v1/patients/${selectedAdmissionPatient.id}/admissions`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(Object.fromEntries(new FormData(admissionForm)))
    });
    if (!response.ok) throw await readApiError(response, "Could not admit patient.");
    admissionForm.reset();
    admissionStatus.textContent = "Patient admitted.";
    await loadAdmissions(selectedAdmissionPatient);
  } catch (error) {
    admissionStatus.textContent = error.message;
    admissionStatus.classList.add("error");
  }
});

dischargeForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  if (!selectedAdmissionPatient || !selectedAdmissionId) return;
  admissionListStatus.textContent = "Saving discharge...";
  admissionListStatus.classList.remove("error");
  try {
    const response = await fetch(`/api/v1/patients/${selectedAdmissionPatient.id}/admissions/${selectedAdmissionId}/discharge`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(Object.fromEntries(new FormData(dischargeForm)))
    });
    if (!response.ok) throw await readApiError(response, "Could not discharge admission.");
    dischargeForm.reset();
    cancelDischarge();
    admissionListStatus.textContent = "Admission discharged.";
    await loadAdmissions(selectedAdmissionPatient);
  } catch (error) {
    admissionListStatus.textContent = error.message;
    admissionListStatus.classList.add("error");
  }
});

document.querySelectorAll("[data-view]").forEach((button) => {
  button.addEventListener("click", () => showView(button.dataset.view));
});
document.querySelectorAll("[data-view-target]").forEach((button) => {
  button.addEventListener("click", () => showView(button.dataset.viewTarget));
});
document.querySelectorAll("[data-view-link]").forEach((link) => {
  link.addEventListener("click", (event) => {
    event.preventDefault();
    showView(link.dataset.viewLink);
  });
});
mobileMenuButton.addEventListener("click", () => sidebar.classList.toggle("open"));
document.querySelector("#refresh-button").addEventListener("click", loadPatients);
cancelButton.addEventListener("click", endEdit);
cancelDischargeButton.addEventListener("click", cancelDischarge);

loginForm.addEventListener("submit", async (event) => {
  event.preventDefault();
  loginStatus.textContent = "Signing in...";
  loginStatus.classList.remove("error");
  try {
    const response = await fetch("/api/v1/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(Object.fromEntries(new FormData(loginForm)))
    });
    if (!response.ok) throw await readApiError(response, "Invalid email or password.");
    openWorkspace(await response.json());
  } catch (error) {
    loginStatus.textContent = error.message;
    loginStatus.classList.add("error");
  }
});

logoutButton.addEventListener("click", async () => {
  const response = await fetch("/api/v1/auth/logout", { method: "POST" });
  if (!response.ok) {
    loginStatus.textContent = "Could not sign out.";
    loginStatus.classList.add("error");
    return;
  }
  closeWorkspace();
});

function beginEdit(patient) {
  showView("patients-view");
  editingPatientId = patient.id;
  form.elements.fullName.value = patient.fullName;
  form.elements.birthDate.value = patient.birthDate;
  form.elements.documentNumber.value = patient.documentNumber;
  formTitle.textContent = "Edit patient";
  submitButton.textContent = "Update patient";
  cancelButton.classList.remove("hidden");
  form.elements.fullName.focus();
}

function endEdit() {
  editingPatientId = null;
  formTitle.textContent = "Register patient";
  submitButton.textContent = "Save patient";
  cancelButton.classList.add("hidden");
}

async function deletePatient(patient) {
  if (!window.confirm(`Delete ${patient.fullName}?`)) return;
  try {
    const response = await fetch(`/api/v1/patients/${patient.id}`, { method: "DELETE" });
    if (!response.ok) throw await readApiError(response, "Could not delete patient.");
    if (editingPatientId === patient.id) {
      form.reset();
      endEdit();
    }
    if (selectedTriagePatient?.id === patient.id) clearTriage();
    if (selectedAdmissionPatient?.id === patient.id) clearAdmission();
    await loadPatients();
  } catch (error) {
    listStatus.textContent = error.message;
    listStatus.classList.add("error");
  }
}

function selectTriagePatient(patient) {
  selectedTriagePatient = patient;
  showView("triage-view");
  triagePatient.textContent = patient.fullName;
  triageStatus.textContent = "";
  loadTriage(patient);
}

async function loadTriage(patient) {
  triageListStatus.textContent = "Loading triage history...";
  triageListStatus.classList.remove("error");
  try {
    const response = await fetch(`/api/v1/patients/${patient.id}/triage`);
    if (!response.ok) throw await readApiError(response, "Could not load triage history.");
    const triages = await response.json();
    triageList.replaceChildren();
    triages.forEach((triage) => {
      const item = document.createElement("li");
      item.innerHTML = `<strong>${riskLabel(triage.riskLevel)}</strong>
        <span>${escapeHtml(triage.chiefComplaint)}</span>
        <span>${new Date(triage.recordedAtUtc).toLocaleString()}</span>`;
      triageList.append(item);
    });
    triageListStatus.textContent = triages.length ? "" : "No triage records yet.";
  } catch (error) {
    triageListStatus.textContent = error.message;
    triageListStatus.classList.add("error");
  }
}

function clearTriage() {
  selectedTriagePatient = null;
  triageForm.reset();
  triageList.replaceChildren();
  triagePatient.textContent = "Choose a patient from the registry";
  triageStatus.textContent = "";
  triageListStatus.textContent = "";
}

function riskLabel(riskLevel) {
  return ({ 1: "Blue - non-urgent", 2: "Green - low risk", 3: "Yellow - needs attention", 4: "Red - emergency" }[riskLevel] || "Unknown");
}

function selectAdmissionPatient(patient) {
  selectedAdmissionPatient = patient;
  showView("admissions-view");
  admissionPatient.textContent = patient.fullName;
  admissionStatus.textContent = "";
  loadAdmissions(patient);
}

async function loadAdmissions(patient) {
  admissionListStatus.textContent = "Loading admission history...";
  admissionListStatus.classList.remove("error");
  try {
    const response = await fetch(`/api/v1/patients/${patient.id}/admissions`);
    if (!response.ok) throw await readApiError(response, "Could not load admission history.");
    const admissions = await response.json();
    admissionList.replaceChildren();
    admissions.forEach((admission) => {
      const item = document.createElement("li");
      item.innerHTML = `<strong>${admissionStatusLabel(admission.status)}</strong>
        <span>Bed: ${escapeHtml(admission.bedNumber)}</span>
        <span>Admitted: ${new Date(admission.admittedAtUtc).toLocaleString()}</span>
        ${admission.dischargedAtUtc
          ? `<span>Discharged: ${new Date(admission.dischargedAtUtc).toLocaleString()}</span>
             <span>Notes: ${escapeHtml(admission.dischargeNotes)}</span>`
          : `<button class="discharge-button" type="button">Discharge</button>`}`;
      item.querySelector(".discharge-button")?.addEventListener("click", () => beginDischarge(admission.id));
      admissionList.append(item);
    });
    admissionListStatus.textContent = admissions.length ? "" : "No admissions registered yet.";
  } catch (error) {
    admissionListStatus.textContent = error.message;
    admissionListStatus.classList.add("error");
  }
}

function beginDischarge(admissionId) {
  selectedAdmissionId = admissionId;
  dischargeForm.classList.remove("hidden");
  dischargeForm.elements.dischargeNotes.focus();
}

function cancelDischarge() {
  selectedAdmissionId = null;
  dischargeForm.reset();
  dischargeForm.classList.add("hidden");
}

function clearAdmission() {
  selectedAdmissionPatient = null;
  admissionForm.reset();
  cancelDischarge();
  admissionList.replaceChildren();
  admissionPatient.textContent = "Choose a patient from the registry";
  admissionStatus.textContent = "";
  admissionListStatus.textContent = "";
}

function admissionStatusLabel(status) {
  return status === 1 ? "Active admission" : "Discharged";
}

function escapeHtml(value) {
  return String(value).replace(/[&<>"']/g, (character) => ({
    "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;"
  }[character]));
}

async function readApiError(response, fallback) {
  try {
    const problem = await response.json();
    return new Error(problem.detail || problem.title || problem.message || fallback);
  } catch {
    return new Error(fallback);
  }
}

topbarDate.textContent = new Intl.DateTimeFormat("en", {
  weekday: "short",
  month: "short",
  day: "numeric"
}).format(new Date());

if ("serviceWorker" in navigator) {
  window.addEventListener("load", () => {
    navigator.serviceWorker.register("/sw.js").catch(() => {
      // The application remains fully usable when offline installation is unavailable.
    });
  });
}
