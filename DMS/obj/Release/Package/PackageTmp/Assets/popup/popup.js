$(document).ready(function() {
	$('.popup-with-form').magnificPopup({
		type: 'inline',
		preloader: false,
		focus: '#name',

		// When elemened is focused, some mobile browsers in some cases zoom in
		// It looks not nice, so we disable it:
		callbacks: {
			beforeOpen: function() {
				if($(window).width() < 700) {
					this.st.focus = false;
				} else {
					this.st.focus = '#name';
				}
			}
		}
	});
	$('#updatePassword').click(function(e){
		e.preventDefault();
	})
});


function updatePass()
{			
	var curPassword=document.getElementById('inputPassword0').value;
	var newpassword=document.getElementById('inputPassword1').value;
	var retypepassword=document.getElementById('inputPassword2').value;
	curPassword=curPassword.trim();
	newpassword=newpassword.trim();	
	retypepassword=retypepassword.trim();	
	
	//alert(curPassword+'-'+newpassword+'-'+retypepassword)
	
	if(curPassword=="")
	{
		document.getElementById('Error-text').innerHTML="Enter Valid Password...";
		document.getElementById('inputPassword0').focus();
		return false;					
	}
	if(newpassword=="")
	{
		document.getElementById('Error-text').innerHTML="Enter New Password...";
		document.getElementById('inputPassword1').focus();
		return false;					
	}
	if(retypepassword=="")
	{
		document.getElementById('Error-text').innerHTML="Retype New Password...";
		document.getElementById('inputPassword2').focus();
		return false;					
	}
	if(newpassword!=retypepassword)
	{
		document.getElementById('Error-text').innerHTML="The Password doesnot match...";
		document.getElementById('inputPassword1').focus();
		return false;					
	}				
	if(curPassword!="" && (newpassword!=""&& retypepassword!=""))
	{
		//alert("ryrey")
		$.ajax({
			type	:	"POST",
			url		:	"ajaxProcess.php",
			data	:	{flag:"updatePassword","curPass":curPassword,"newPass":newpassword},
			success	:	function(data){
							//alert(data);
							//return false;
							if(data!="error" && data!="Invalid")
							{
								alert("Successfully Updated");
								location.reload();
							}
							else
							{
								//alert('pop');
								document.getElementById('Error-text').innerHTML="Enter Valid Current Password...";
								return false;
							}						
						},
			error	:	function(data)
						{
							alert("	Error");
						}
		});	
		return false;
	}
}
