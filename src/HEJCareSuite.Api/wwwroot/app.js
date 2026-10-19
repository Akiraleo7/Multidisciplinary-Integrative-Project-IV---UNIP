const form = document.querySelector("#patient-form");
const formStatus = document.querySelector("#form-status");
const listStatus = document.querySelector("#list-status");
const patientList = document.querySelector("#patient-list");

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
        <span>${patient.birthDate} · ${escapeHtml(patient.documentNumber)}</span>`;
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
    const response = await fetch("/api/v1/patients", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(data)
    });
    if (!response.ok) throw await readApiError(response, "Could not save patient.");
    form.reset();
    formStatus.textContent = "Patient registered.";
    await loadPatients();
  } catch (error) {
    formStatus.textContent = error.message;
    formStatus.classList.add("error");
  }
});

document.querySelector("#refresh-button").addEventListener("click", loadPatients);

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

loadPatients();
