(function() {
    window.InitiateManagerSelect = function () {
        document.getElementById('manager-save-button').addEventListener('click', function (ev) {
            var managerSelect = document.getElementById('manager-select'),
                managerList = document.getElementById('manager-list'),
                index = managerList.children.length,
                li = document.createElement('li'),
                input = document.createElement('input'),
                selectedManagerId = parseInt(managerSelect.value, 10)
                alreadyExists = false;

            if (!Number.isSafeInteger(selectedManagerId)) {
                return;
            }

            if (managerList.children.length > 0) {
                alreadyExists = Array.prototype.slice.call(managerList.children).some(function (element) {
                    return selectedManagerId === parseInt(element.querySelector('input[type="hidden"]').value, 10);
                });
            }

            if (alreadyExists === true) {
                return;
            }

            input.type = 'hidden';
            input.name = `Managers[${index}]`;
            input.value = selectedManagerId;

            li.className = 'list-group-item';
            li.innerText = managerSelect.options[managerSelect.selectedIndex].text;

            li.appendChild(input);

            managerList.appendChild(li);
        });
    };
})();