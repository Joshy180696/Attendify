// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// search-box open close js code
let navbar = document.querySelector(".navbar");
let searchBox = document.querySelector(".search-box .bx-search");
let inputField = document.querySelector(".search-box input");
// let searchBoxCancel = document.querySelector(".search-box .bx-x");
if (searchBox && inputField) {
    if (!inputField.disabled) {
        searchBox.addEventListener("click", () => {
            navbar.classList.toggle("showInput");
            if (navbar.classList.contains("showInput")) {
                searchBox.classList.replace("bx-search", "bx-x");
                inputField.focus(); // Focus the input field to start typing
            } else {
                searchBox.classList.replace("bx-x", "bx-search");
            }
        });
    }

}

// Trigger search only when 'Enter' is pressed
inputField.addEventListener("keypress", (e) => {
    if (e.key === "Enter") {
        performSearch(inputField.value); // Call the search function when 'Enter' is pressed
    }
});
// Function to perform the search
function performSearch(query) {
   
    console.log("Searching for:", query);
    loadEventsByDate(query); // Call your actual search function or API
    inputField.value = "";
}

// sidebar open close js code
let navLinks = document.querySelector(".nav-links");
let menuOpenBtn = document.querySelector(".navbar .bx-menu");
let menuCloseBtn = document.querySelector(".nav-links .bx-x");
menuOpenBtn.onclick = function () {
    navLinks.style.left = "0";
}
menuCloseBtn.onclick = function () {
    navLinks.style.left = "-100%";
}
// sidebar submenu open close js code
let htmlcssArrow = document.querySelector(".htmlcss-arrow");
htmlcssArrow.onclick = function () {
    navLinks.classList.toggle("show1");
}
let moreArrow = document.querySelector(".more-arrow");
if (moreArrow) {
    moreArrow.onclick = function () {
        navLinks.classList.toggle("show2");
    }
}

let jsArrow = document.querySelector(".js-arrow");
if (jsArrow) {
    jsArrow.onclick = function () {
        navLinks.classList.toggle("show3");
    }
}

