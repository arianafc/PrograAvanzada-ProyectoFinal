const hideToggle = document.querySelector(".hide-toggle");
const menuToggle = document.querySelector(".menu-toggle");
const navLinks = document.querySelector(".nav-links");
hideToggle.addEventListener("click", () => {
  navLinks.classList.toggle("hide");
});
menuToggle.addEventListener("click", () => {
  if (navLinks.classList.contains("hide")) {
    navLinks.classList.remove("hide");
    navLinks.classList.toggle("show");
  }
});