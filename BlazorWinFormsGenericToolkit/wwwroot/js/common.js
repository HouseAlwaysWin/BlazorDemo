
// scroll 到指定位置
function scrollToElementId(id) {
    let el = document.querySelector(`#${id}`)
    el.scrollIntoView({
        "block": "end"
    });
}

function scrollToEndById(id) {
    let el = document.querySelector(`#${id}`);
    el.scrollTop = el.scrollHeight;
}