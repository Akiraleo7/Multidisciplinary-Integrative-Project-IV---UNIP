// VLibras setup
const vlibrasScript = document.createElement("script");
vlibrasScript.src = "https://vlibras.gov.br/app/vlibras-plugin.js";
vlibrasScript.async = true;
document.head.appendChild(vlibrasScript);
vlibrasScript.onload = () => {
  new window.VLibras.Widget('https://vlibras.gov.br/app');
};
