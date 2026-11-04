const form = document.querySelector("#patient-form");
const formStatus = document.querySelector("#form-status");
const listStatus = document.querySelector("#list-status");
const patientList = document.querySelector("#patient-list");
const formTitle = document.querySelector("#form-title");
const submitButton = document.querySelector("#submit-button");
const cancelButton = document.querySelector("#cancel-button");
const loginForm = document.querySelector("#login-form");
const loginStatus = document.querySelector("#login-status");
const logoutButton = document.querySelector("#logout-button");
const triageSection = document.querySelector("#triage-section");
const triageForm = document.querySelector("#triage-form");
const triagePatient = document.querySelector("#triage-patient");
const triageStatus = document.querySelector("#triage-status");
const triageListStatus = document.querySelector("#triage-list-status");
const triageList = document.querySelector("#triage-list");
let editingPatientId = null;
let selectedTriagePatient = null;

async function loadPatients() {
  listStatus.textContent = "Loading patients...";
  listStatus.classList.remove("error");
  try {
    const response = await fetch("/api/v1/patients");
    if (!response.ok) throw await readApiError(response, "Could not load patients.");
    const patients = await response.json();
    patientList.replaceChildren();
    patients.forEach((patient) => {
      const item = document.createElement("li");
      item.innerHTML = `<strong>${escapeHtml(patient.fullName)}</strong>
        <span>${patient.birthDate} · ${escapeHtml(patient.documentNumber)}</span>
        <span class="patient-actions">
          <button class="edit-button" type="button">Edit</button>
          <button class="delete-button" type="button">Delete</button>
          <button class="triage-button" type="button">Triage</button>
        </span>`;
      item.querySelector(".edit-button").addEventListener("click", () => beginEdit(patient));
      item.querySelector(".delete-button").addEventListener("click", () => deletePatient(patient));
      item.querySelector(".triage-button").addEventListener("click", () => selectTriagePatient(patient));
      patientList.append(item);
    });
    listStatus.textContent = patients.length ? "" : "No patients registered yet.";
  } catch (error) {
    listStatus.textContent = error.message;
    listStatus.classList.add("error");
  }
}

form.addEventListener("submit", async (event) => {
  event.preventDefault();
  formStatus.textContent = "Saving...";
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
  if (!selectedTriagePatient) return;
  triageStatus.textContent = "Saving...";
  triageStatus.classList.remove("error");
  const data = Object.fromEntries(new FormData(triageForm));
  data.riskLevel = Number(data.riskLevel);
  try {
    const response = await fetch(
      `/api/v1/patients/${selectedTriagePatient.id}/triage`,
      {
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

document.querySelector("#refresh-button").addEventListener("click", loadPatients);
cancelButton.addEventListener("click", endEdit);
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
    const user = await response.json();
    loginStatus.textContent = `Signed in as ${user.role}.`;
    await loadPatients();
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
  loginStatus.textContent = "Signed out.";
  listStatus.textContent = "Sign in to load patients.";
  patientList.replaceChildren();
  clearTriage();
  endEdit();
});

function beginEdit(patient) {
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
    await loadPatients();
  } catch (error) {
    listStatus.textContent = error.message;
    listStatus.classList.add("error");
  }
}

function selectTriagePatient(patient) {
  selectedTriagePatient = patient;
  triageSection.classList.remove("hidden");
  triagePatient.textContent = `Patient: ${patient.fullName}`;
  triageStatus.textContent = "";
  loadTriage(patient);
  triageSection.scrollIntoView({ behavior: "smooth", block: "start" });
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
  triageSection.classList.add("hidden");
  triageForm.reset();
  triageList.replaceChildren();
  triagePatient.textContent = "Select a patient to manage triage.";
  triageStatus.textContent = "";
  triageListStatus.textContent = "";
}

function riskLabel(riskLevel) {
  return ({
    1: "Blue",
    2: "Green",
    3: "Yellow",
    4: "Red"
  }[riskLevel] || "Unknown");
}

function escapeHtml(value) {
  return value.replace(/[&<>"']/g, (character) => ({
    "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;"
  }[character]));
}

async function readApiError(response, fallback) {
  try {
    const problem = await response.json();
    return new Error(problem.detail || problem.title || fallback);
  } catch {
    return new Error(fallback);
  }
}

listStatus.textContent = "Sign in to load patients.";
