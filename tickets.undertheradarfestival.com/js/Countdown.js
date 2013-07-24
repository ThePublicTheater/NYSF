var _countDowncontainer = 0;
var _currentSeconds = 0;

function ActivateCountDown(strContainerID, initialValue) {
    _countDowncontainer = document.getElementById(strContainerID);

    if (!_countDowncontainer) {
        alert("count down error: container does not exist: " + strContainerID +
"\nmake sure html element with this ID exists");
        return;
    }

    SetCountdownText(initialValue);
    window.setTimeout("CountDownTick()", 1000);
}

function CountDownTick() {
    if (_currentSeconds <= 0) {
        alert("Your cart has expired.");
        return;
    }

    SetCountdownText(_currentSeconds - 1);
    window.setTimeout("CountDownTick()", 1000);
}

function SetCountdownText(seconds) {
    //store:
    _currentSeconds = seconds;

    //get minutes:
    var minutes = parseInt(seconds / 60);

    //shrink:
    seconds = (seconds % 60);

    //get hours:
    var hours = parseInt(minutes / 60);

    //shrink:
    minutes = (minutes % 60);

    //build text:
    var strText = "";
    if (hours > 0)
        strText = AddZero(hours) + ":";
    strText += AddZero(minutes) + ":" + AddZero(seconds);

    //apply:
    _countDowncontainer.innerHTML = strText;
}

function AddZero(num) {
    return ((num >= 0) && (num < 10)) ? "0" + num : num + "";
}