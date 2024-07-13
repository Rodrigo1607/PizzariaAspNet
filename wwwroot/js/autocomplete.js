document.addEventListener("DOMContentLoaded", function () {
    const input = document.getElementById("pesquisar");
    const suggestionsBox = document.getElementById("autocomplete-suggestions");

    input.addEventListener("input", function () {
        const query = input.value;

        if (query.length >= 3) {
            fetch(`/Pizza/BuscarSugestoes?termo=${query}`)
                .then(response => response.json())
                .then(data => {
                    suggestionsBox.innerHTML = "";
                    data.forEach(pizza => {
                        const suggestion = document.createElement("div");
                        suggestion.classList.add("autocomplete-suggestion");
                        suggestion.textContent = pizza.sabor; // Ajuste conforme a propriedade do modelo PizzaModel
                        suggestion.addEventListener("click", () => {
                            input.value = pizza.sabor; // Preenche o campo de entrada com a seleção
                            suggestionsBox.innerHTML = ""; // Limpa as sugestões
                        });
                        suggestionsBox.appendChild(suggestion);
                    });
                })
                .catch(error => console.error('Error:', error));
        } else {
            suggestionsBox.innerHTML = "";
        }
    });

    document.addEventListener("click", function (e) {
        if (!suggestionsBox.contains(e.target) && e.target !== input) {
            suggestionsBox.innerHTML = "";
        }
    });
});