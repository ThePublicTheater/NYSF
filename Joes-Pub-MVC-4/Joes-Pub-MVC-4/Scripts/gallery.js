current_gallery_photo = 0;
document.getElementById('gallery_photo_0').style.display = '';

function next_gallery_photo() {
	var count = parseInt(document.getElementById('gallery_count').value);
	var next_gallery_photo = (current_gallery_photo + 1) % count;
	document.getElementById('gallery_photo_' + current_gallery_photo).style.display = 'none';
	document.getElementById('gallery_photo_' + next_gallery_photo).style.display = '';
	current_gallery_photo = next_gallery_photo;
}

function prev_gallery_photo() {
	var count = parseInt(document.getElementById('gallery_count').value);
	var prev_gallery_photo = current_gallery_photo - 1;
	if(prev_gallery_photo < 0) {
		prev_gallery_photo = count - 1;
	}
	document.getElementById('gallery_photo_' + current_gallery_photo).style.display = 'none';
	document.getElementById('gallery_photo_' + prev_gallery_photo).style.display = '';
	current_gallery_photo = prev_gallery_photo;
}