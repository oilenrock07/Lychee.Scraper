function getInnerText(element) {
    return element.innerText; //add more logic here like trim()
}

function getInputValue(element) {
    return element.value;
}

function getAttribute(element, attributeName) {
    return element.getAttribute(attributeName);
}

function dropDownSelectedText(element) {
    return element.options[element.selectedIndex].text;

}