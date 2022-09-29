using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailHandler : MonoBehaviour
{
    public InputField bodyMessage;
    public InputField recipientEmail;
	public InputField subjectLine;

	public void SendEmail()
    {
        try {
        
            MailMessage mail = new MailMessage();
            SmtpClient smtpC = new SmtpClient("smtp.gmail.com");
            //From address to send email
            mail.From = new MailAddress("wacrhapptest@gmail.com");
            //To address to send email
            mail.To.Add(recipientEmail.text);
            mail.Body = bodyMessage.text;
            mail.Subject = subjectLine.text;
            smtpC.Port = 587;
            //Credentials for From address
            smtpC.Credentials =(System.Net.ICredentialsByHost) new System.Net.NetworkCredential("wacrhapptest@gmail.com", "yxfffkqosdkicftt");
            smtpC.EnableSsl = true;
            smtpC.Send(mail);

            //Change Console.Writeline to Debug.Log 
            Debug.Log ("Message sent successfully");
        }
        catch (Exception e)
        {
            Debug.Log(e.GetBaseException());
            //You don't need or use Console.ReadLine();
        }
    }

}