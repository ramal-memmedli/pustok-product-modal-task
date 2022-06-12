let modalArea = document.getElementById("modalRenderArea");
let openModalButtons = document.querySelectorAll(".open-modal-btn");

openModalButtons.forEach(openModalBtn => {
    openModalBtn.addEventListener("click", async function (event) {
        event.preventDefault();
        let productId = openModalBtn.getAttribute("data-id");
        let response = await fetch(`/home/GetProduct/${productId}`);
        let data = await response.text();
        modalArea.innerHTML = data;
    })
})