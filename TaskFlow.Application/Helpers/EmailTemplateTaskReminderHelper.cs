using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Helpers
{
    public static class EmailTemplateTaskReminderHelper
    {
        public static string GenerateTaskReminderEmail(
            string userName,
            string taskTitle,
            DateTime dueDate,
            string priority,
            string category,
            string taskLink,
            string from
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
    .header {{ font-size: 20px; font-weight: bold; color: #d97706; margin-bottom: 10px; }}
    .task-title {{ font-size: 18px; font-weight: bold; color: #dc2626; margin: 10px 0; }}
    .details {{ font-size: 14px; color: #555; margin: 5px 0; }}
    .footer {{ margin-top: 20px; font-size: 12px; color: #888; }}
    .btn {{ display: inline-block; margin-top: 15px; padding: 10px 15px; 
            background-color: #dc2626; color: white; text-decoration: none; border-radius: 5px; }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>⏰ Task Reminder</div>
    <p>Hello <strong>{userName}</strong>,</p>
    <p>The following task is approaching its deadline. Please take action:</p>
    
    <div class='task-title'>⚠️ {taskTitle}</div>
    <div class='details'><strong>Due Date:</strong> {dueDate:yyyy-MM-dd HH:mm tt}</div>
    <div class='details'><strong>Priority:</strong> {priority}</div>
    <div class='details'><strong>Category:</strong> {category}</div>
    
    <a href='{taskLink}' class='btn'>View Task</a>
    
    <div class='details'><strong>From:</strong> {from}</div>

    <div class='footer'>
      This is an automated reminder from TaskFlow. Please do not reply directly to this email.
    </div>
  </div>
</body>
</html>";
        }
    }
}
