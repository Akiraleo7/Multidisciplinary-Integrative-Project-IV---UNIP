const form = document.querySelector("#patient-form");
const formStatus = document.querySelector("#form-status");
const listStatus = document.querySelector("#list-status");
const patientList = document.querySelector("#patient-list");
const formTitle = document.querySelector("#form-title");
const submitButton = document.querySelector("#submit-button");
const cancelButton = document.querySelector("#cancel-button");
const loginForm = document.querySelector("#login-form");
const loginStatus = document.querySelector("#login-status");
let editingPatientId = null;

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
        </span>`;
      item.querySelector(".edit-button").addEventListener("click", () => beginEdit(patient));
      item.querySelector(".delete-button").addEventListener("click", () => deletePatient(patient));
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
    await loadPatients();
  } catch (error) {
    listStatus.textContent = error.message;
    listStatus.classList.add("error");
  }
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
