var districtArray = new Array("Ariyalur", "Chennai", "Coimbatore", "Cuddalore", "Dharmapuri", "Dindigul", "Erode", "Kancheepuram","Kanniyakumari","Karur", "Krishnagiri", "Madurai", "Nagapattinam","Namakkal", "Perambalur", "Pudukkottai", "Ramanathapuram","Salem", "Sivagangai", "Thanjavur", "TheNilgiris","Theni", "Thiruvallur", "Thiruvarur", "Tutucorin","Tiruchirappalli","Tirunelveli", "Tiruppur", "Tiruvannamalai","Vellore", "Villupuram", "Virudhunagar");

function print_district(dist_id){
	// given the id of the <select> tag as function argument, it inserts <option> tags
	var option_str = document.getElementById(dist_id);
	option_str.length=0;
	option_str.options[0] = new Option('Select District','');
	option_str.selectedIndex = 0;
	for (var i=0; i<districtArray.length; i++) {
		option_str.options[option_str.length] = new Option(districtArray[i],districtArray[i]);
	}
}