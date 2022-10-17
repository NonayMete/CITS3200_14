using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TMPro;

public class EmailHandler : MonoBehaviour
{
    public InputField bodyMessage;
	public InputField subjectLine;
	public TMP_Dropdown recipientEmailSelect;
    

	public void SendEmail()
    {
        try {
        
            MailMessage mail = new MailMessage();
            SmtpClient smtpC = new SmtpClient("smtp.gmail.com");
            //From address to send email
            mail.From = new MailAddress("wacrhapptest@gmail.com");

            mail.To.Add(recipientEmailSelect.options[recipientEmailSelect.value].text);
            mail.Body = "From: "+ AuthManager.email_global + " " + AuthManager.name_global + "\n" + bodyMessage.text;
            mail.Subject = subjectLine.text;
			
			if (EmailFileHandler.destinationPath != null){
				Attachment attachment = null;
				attachment = new Attachment(EmailFileHandler.destinationPath);
				mail.Attachments.Add(attachment);
			}
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