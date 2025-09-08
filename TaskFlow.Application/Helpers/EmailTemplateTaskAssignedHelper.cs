using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Helpers
{
	public static class EmailTemplateTaskAssignedHelper
	{
		public static string GenerateTaskAssignedEmail(
			string taskTitle,
			string taskDescription,
			DateTime dueDate,
			string priority,
			string category,
			string taskLink,
            string from,
            string fromEmail
            
            )
		{
			return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <style>
    body {{ font-family: Arial, sans-serif; background-color: #f4f4f7; padding: 20px; }}
    .container {{ background-color: #ffffff; border-radius: 8px; padding: 20px; 
                  box-shadow: 0 2px 5px rgba(0,0,0,0.1); }}
    .header {{ font-size: 20px; font-weight: bold; color: #333; margin-bottom: 10px; }}
    .task-title {{ font-size: 18px; font-weight: bold; color: #0073e6; margin: 10px 0; }}
    .details {{ font-size: 14px; color: #555; margin: 5px 0; }}
    .footer {{ margin-top: 20px; font-size: 12px; color: #888; }}
    .btn {{ display: inline-block; margin-top: 15px; padding: 10px 15px; 
            background-color: #0073e6; color: white; text-decoration: none; border-radius: 5px; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>📌 New Task Assigned to You</div>
    <p>Hello,</p>
    <p>You have been assigned a new task. Here are the details:</p>
    
    <div class='task-title'>Task: {taskTitle}</div>
    <div class='details'><strong>Description:</strong> {taskDescription}</div>
    <div class='details'><strong>Due Date:</strong> {dueDate:yyyy-MM-dd hh:mm tt}</div>
    <div class='details'><strong>Priority:</strong> {priority}</div>
    <div class='details'><strong>Category:</strong> {category}</div>
    
    <a href='{taskLink}' class='btn'>View Task</a>
     
    <div class='details'><strong>From:</strong> {from}</div>
    <div class='details'><strong>Email:</strong> {fromEmail}</div>

    <div class='footer'>
      This is an automated message from TaskFlow. Please do not reply directly to this email.
    </div>
  </div>
</body>
</html>";
		}
	}

}
