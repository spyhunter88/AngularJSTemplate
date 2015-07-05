$.fn.insertAt = function (element, index) {
    var lastIndex = this.children().size()
    if (index < 0) {
        index = Math.max(0, lastIndex + 1 + index)
    }
    this.append(element)
    if (index < lastIndex) {
        this.children().eq(index).before(this.children().last())
    }
    return this;
}