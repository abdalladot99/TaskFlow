using System;

namespace TaskFlow.Application.Helpers
{
	public static class EmailTemplateNotificationHelper
	{
		public static string GenerateNotificationEmail(
			string title,
			string message,
			DateTime createdAt
		)
		{
			return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <style>
    body {{
      font-family: 'Segoe UI', Arial, sans-serif;
      background: #eef2f7;
      margin: 0;
      padding: 30px;
    }}
    .container {{
      background: #fff;
      border-radius: 14px;
      max-width: 600px;
      margin: auto;
      padding: 25px 30px;
      box-shadow: 0 6px 20px rgba(0,0,0,0.08);
    }}
    .header {{
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 10px;
      font-size: 22px;
      font-weight: bold;
      color: #1e88e5;
      margin-bottom: 20px;
    }}
    .title {{
      font-size: 18px;
      font-weight: 600;
      color: #333;
      margin-bottom: 12px;
    }}
    .message {{
      font-size: 15px;
      color: #555;
      line-height: 1.6;
      margin-bottom: 20px;
    }}
    .time {{
      font-size: 13px;
      color: #888;
      text-align: right;
      margin-top: -10px;
    }}
    .footer {{
      margin-top: 30px;
      font-size: 12px;
      color: #aaa;
      text-align: center;
      border-top: 1px solid #eee;
      padding-top: 15px;
    }}
    .badge {{
      display: inline-block;
      background: #1e88e5;
      color: #fff;
      font-size: 12px;
      padding: 5px 10px;
      border-radius: 20px;
      margin-bottom: 10px;
    }}
  </style>
</head>
<body>
  <div class='container'>
    <div class='header'>🔔 TaskFlow Notification</div>
    <div class='badge'>New</div>
    <div class='title'>{title}</div>
    <div class='message'>{message}</div>
    <div class='time'>📅 {createdAt:yyyy-MM-dd HH:mm}</div>
    <div class='footer'>
      This is an automated notification from <strong>TaskFlow</strong>.<br/>
      Please do not reply directly to this email.
    </div>
  </div>
</body>
</html>";
		}
	}
}
