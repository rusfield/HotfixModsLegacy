// Used to limit amount of JS calls from Blazor
/* Usage in Blazor:
    var functionSpecs = new object[]
    {
        new { name = "addAndRemoveClass", args = new object[] { id, "loading", animationDurationMs } },
        new { name = "toggleDropdownHeight", args = new object[] { id, true, dropdownHeight, animationDurationMs } },
        new { name = "toggleDropdownOpacity", args = new object[] { parentContainerId, true, animationDurationMs } }
    };
    await JSRuntime.InvokeVoidAsync("executeMultipleFunctions", new object[] { functionSpecs });
*/
window.executeMultipleFunctions = (functions) => {
    functions.forEach(funcSpec => {
        const func = window[funcSpec.name];
        if (typeof func === 'function') {
            try {
                func.apply(null, funcSpec.args);
            } catch (err) {
                console.error(`Error executing function ${funcSpec.name}: ${err.message}`);
            }
        } else {
            console.error(`Function ${funcSpec.name} not found`);
        }
    });
};



// Used by Menu to scroll to item
window.scrollToElementInContainer = (containerId, elementId, scrollDuration) => {
    const container = document.getElementById(containerId);
    const element = document.getElementById(elementId);
    if (container && element) {
        const start = container.scrollTop;
        const end = element.offsetTop;
        const change = end - start;
        const duration = scrollDuration; // Duration in ms
        let startTimestamp;

        const easeInOutQuad = (t, b, c, d) => {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        };

        const animateScroll = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const elapsed = timestamp - startTimestamp;
            const progress = Math.min(elapsed / duration, 1);
            container.scrollTop = start + easeInOutQuad(progress, 0, change, 1);
            if (progress < 1) {
                window.requestAnimationFrame(animateScroll);
            }
        };

        window.requestAnimationFrame(animateScroll);
    }
};


// Focus element by ID
window.focusElement = (elementId) => {
    document.getElementById(elementId).focus();
};

// Select text in element by ID
window.selectElement = (elementId) => {
    setTimeout(function () {
        var element = document.getElementById(elementId);
        element.focus();
        element.select();
        element.scrollLeft = 0; // prevent jumping back and forth for focus/unfocus
    }, 100);
}





// Animate a CSS property of an element
let ongoingAnimations = {};
window.animateProperty = (elementId, property, startValue, endValue, durationMs, unit = '') => {
    const element = document.getElementById(elementId);
    if (!element) return; // If the element does not exist, ignore the request

    // If there's an ongoing animation for this element and this property, cancel it
    if (ongoingAnimations[elementId] && ongoingAnimations[elementId][property]) {
        window.cancelAnimationFrame(ongoingAnimations[elementId][property]);
    } else if (!ongoingAnimations[elementId]) {
        ongoingAnimations[elementId] = {};
    }

    let startTimestamp = null;

    function frame(timestamp) {
        if (startTimestamp === null) startTimestamp = timestamp;
        const elapsed = timestamp - startTimestamp;
        const progress = Math.min(elapsed / durationMs, 1);
        const currentValue = startValue + (endValue - startValue) * progress;
        element.style[property] = currentValue + unit;

        if (progress < 1) {
            ongoingAnimations[elementId][property] = window.requestAnimationFrame(frame);
        } else {
            delete ongoingAnimations[elementId][property];
            if (Object.keys(ongoingAnimations[elementId]).length === 0) {
                delete ongoingAnimations[elementId]; // Cleanup if no more ongoing animations for this element
            }
        }
    }

    ongoingAnimations[elementId][property] = window.requestAnimationFrame(frame);
};

window.toggleDropdownHeight = (elementId, isOpen, height, durationMs) => {
    const startHeight = isOpen ? 0 : height;
    const endHeight = isOpen ? height : 0;
    window.animateProperty(elementId, 'height', startHeight, endHeight, durationMs, 'px'); 
};

window.toggleDropdownOpacity = (elementId, isOpen, durationMs) => {
    const startOpacity = isOpen ? 0 : 1;
    const endOpacity = isOpen ? 1 : 0;
    window.animateProperty(elementId, 'opacity', startOpacity, endOpacity, durationMs);
};



window.addAndRemoveClass = (elementId, className, duration) => {
    let element = document.getElementById(elementId);
    element.classList.add(className);

    setTimeout(() => {
        element.classList.remove(className);
    }, duration);
};

window.addClass = (elementId, className) => {
    let element = document.getElementById(elementId);
    element.classList.add(className);
};

window.removeClass = (elementId, className) => {
    let element = document.getElementById(elementId);
    element.classList.remove(className);
};   

// By disabling drag, Blazor no longer gets an error on drag drop for random text that causes this weird cursor bug.
// https://github.com/MicrosoftEdge/WebView2Feedback/issues/2805
document.addEventListener("dragstart", function (event) {
    event.preventDefault();
});