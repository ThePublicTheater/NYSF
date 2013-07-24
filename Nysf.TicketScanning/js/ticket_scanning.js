$(document).ready(function () {
	var scanTextBox = $("#ScanControls input[type='text']");
	scanTextBox.blur(function () {
		setTimeout(function () {
			scanTextBox.focus();
		}, 300);
	});
	/*scanTextBox.keyup(function () {
	scanTextBox.unbind("keyup");
	setTimeout(function () {
	$("#ScanControls input[type='submit']").click();
	}, 300);
	});*/
	setInterval(function () {
		scanTextBox.focus();
	}, 1000);
	$(window).unload(function () {
		scanTextBox.val("");
	});
	$(window).blur(function () {
		$("#ScannerStatusMsg").html("Click in window to scan.");
	});
	$(window).click(function () {
		$("#ScannerStatusMsg").html("Ready to scan.");
	});
});