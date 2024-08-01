namespace EventMatcha.BackgroundServiceCore.MessageTemplates
{
    public class MessageTemplateServices
    {
        public string GenerateUserRegistrationEmail(string senderEmail, string senderName, string receiverEmail, string receiverName, string otp, DateTime otpExpiredOn, string subject)
        {
            string dateString = otpExpiredOn.ToString("MMMM dd, yyyy HH:mm");

            string htmlTemplate = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>{subject}</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            width: 100%;
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            text-align: center;
                            padding-bottom: 20px;
                        }}
                        .header img {{
                            width: 100px;
                        }}
                        .content {{
                            text-align: center;
                        }}
                        .otp {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #333333;
                        }}
                        .footer {{
                            text-align: center;
                            padding-top: 20px;
                            font-size: 12px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>{subject}</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {receiverName},</p>
                            <p>Thank you for signing up. To verify your email address, please use the following OTP:</p>
                            <p class='otp'>{otp}</p>
                            <p>This OTP is valid until {dateString}.</p>
                        </div>
                        <div class='footer'>
                            <p>If you did not request this, please ignore this email.</p>
                            <p>Regards,</p>
                            <p>{senderName}</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return htmlTemplate;
        }
        public string GenerateUserRegistrationConfirmationEmail(string senderEmail, string senderName, string receiverEmail, string receiverName, string subject)
        {

            string htmlTemplate = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>{subject}</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            width: 100%;
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            text-align: center;
                            padding-bottom: 20px;
                        }}
                        .header img {{
                            width: 100px;
                        }}
                        .content {{
                            text-align: center;
                        }}
                        .otp {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #333333;
                        }}
                        .footer {{
                            text-align: center;
                            padding-top: 20px;
                            font-size: 12px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>{subject}</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {receiverName},</p>
                            <p>Welcome to EventMatcha app</p>
                            <p>Thank you for registering with us. We're excited to have you as part of our community.
                        Your account has been successfully created, and you’re now ready
                        to explore all the features we have to offer</p>

                           
                            <p>Your login details are </p>
                            <p>Email:  {receiverEmail} </p>
                            <p>Password: [as chosen]  </p>

                        </div>
                        <div class='footer'>
                            <p>If you did not request this, please ignore this email.</p>
                            <p>Regards,</p>
                            <p>{senderName}</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return htmlTemplate;
        }

    }
}
