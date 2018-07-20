$(document).ready(function() {
				$('.popup-with-form').magnificPopup({
					type: 'inline',
					preloader: false,
					focus: '#inputUname',

					// When elemened is focused, some mobile browsers in some cases zoom in
					// It looks not nice, so we disable it:
					callbacks: {
						beforeOpen: function() {
							if($(window).width() < 700) {
								this.st.focus = false;
							} else {
								this.st.focus = '#inputUname';
							}
						}
					}
				});
				$('#submitBtn').click(function(e){
					e.preventDefault();
				})
			}); 
 function changePassword()
 {
  
		var old_password 		= $('#oldPassword').val();
		var new_password 		= $('#newPassword').val();
		var renew_password 		= $('#RenewPassword').val();
		if(old_password == "")
		{
			
		//	alert("Please enter old password.");
			//return false;
			blurt
			(
				{  
					title: 'Member',   
					text: 'Please enter old password.',  
					type: 'warning',   
					okButtonText: 'OK',
					idFocus:'oldPassword'						 
				}
			);
		 	return false;		
		}
		if(new_password == "")
		{
			blurt(
				{   
					title: 'Change Password',   
					text: 'Please enter new password.',    
					type: 'warning',   
					okButtonText: 'OK',
					idFocus:'newPassword'						 
				}
			)
			return false;					
		}
		if(renew_password == "")
		{
			blurt(
				{   
					title: 'Change Password',   
					text: 'Please enter re-new password.',   
					type: 'warning',   
					okButtonText: 'OK',
					idFocus:'RenewPassword'						 
				}
			);
			return false;	
			 				
		}
		if(new_password != renew_password)
		{
			blurt(
				{   
					title: 'Change Password',    
					text: 'Password does not match the confirm password',   
					type: 'warning',   
					okButtonText: 'OK',
					idFocus:'RenewPassword'						 
				}
			);
			return false;	
		 
		}		
		else
		{
		
			var userName = "<?php echo $_SESSION['user-name'] ;?>";
			
			var data = new Object();
			
			data['flag'] 	= 'changePassword';
			data['user_name'] 	= userName;
			data['password'] 	= old_password;
			data['newPassword'] = new_password;
			
			$.ajax({
					type	:	"POST",
					url		:	"../Process/changePasswordProcess.php",
					data	:	data,
					success	:	function(successData){	
								 
									if(successData!="Fail")
									{
										 alert(successData);
									}
									else
									{
										alert("Some thing went wrong...");return false;
									}
								},
					error	:	function(errorData)
								{
									alert("Request Failed"+errorData.toSource());
									return false;
								}
			});
		}
 }			

function reset()
	{
		 
		 $('input[type="password"]').val('');
	}  
					
			