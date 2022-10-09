from distutils.command.upload import upload
from tkinter import *
from TkinterDnD2 import *
from pdf2image import convert_from_path
import os
from tqdm import tqdm
import firebase_functions as fb
from PIL import ImageTk, Image

program_path = os.path.dirname(__file__)

ws = TkinterDnD.Tk()
ws.title('WACRH PDF to PNG converter')
ws.geometry('600x600')
#ws.config(bg='#fcb103')
frame = Frame(ws)
frame.place(x=20, y=10)
current_filename = ""
current_pagecount = 0

def convert_to_images(event):
    print("found pdf file")
    filepath = event.data[1:-1]
    extension = event.data[-4:-1]
    if(extension != "pdf"):
        print("ERROR: file must be a pdf")
        return
    filename = filepath.split('/')[-1][0:-4]
    print(filename)
    #convert PDF file
    poppler_path=program_path+r"\poppler-0.68.0\bin"
    #print(poppler_path)
    pages = convert_from_path(filepath, 200, poppler_path=poppler_path)
    if not os.path.isdir(program_path+ '/' + filename):
        os.mkdir(program_path+ '/' + filename)
    i = 0
    for page in tqdm(pages):
        page.save(filename+'/'+str(i)+'.png', 'PNG')
        i+=1
    global current_filename
    current_filename=filename
    global current_pagecount
    current_pagecount = i
    display_images(filename)

textarea = Text(frame, height=18, width=40)
textarea.insert("end", "Drag a PDF document here to convert \n to images.")
textarea.pack(side=LEFT)
textarea.drop_target_register(DND_FILES)
textarea.dnd_bind('<<Drop>>', convert_to_images)
sbv = Scrollbar(frame, orient=VERTICAL)
sbv.pack(side=RIGHT, fill=Y)
textarea.configure(yscrollcommand=sbv.set)
sbv.config(command=textarea.yview)

#download directory file from firebase
fb.download_file("directory.txt")
folders = []
subfolders = []
with open("directory.txt", "r") as file:
    temp = []
    for line in file:
        stripped_line = line.strip()
        if stripped_line[0] != '-':
            folders.append(stripped_line)
            subfolders.append(temp[:])
            temp = []
        else:
            temp.append(stripped_line[1:])
subfolders.pop(0)

folders.append("All Folders")
print(folders)
print(subfolders)

w = Label(ws, text="Select Sub Folder")
w.place(x=400, y=80)
global sub_variable
sub_variable = StringVar(ws)
sub_variable.set(subfolders[0][0]) # default value
global sub_folders
sub_folders = OptionMenu(ws, sub_variable, *subfolders[0])
sub_folders.place(x=400, y=100)

def update_sub_folders(parent):
    print(parent)
    index = folders.index(parent)
    print(index)
    menu = sub_folders["menu"]
    menu.delete(0, "end")
    for string in subfolders[index]:
        menu.add_command(label=string, 
                            command=lambda value=string: sub_variable.set(value))

w = Label(ws, text="Select Parent Folder")
w.place(x=400, y=30)
parent_variable = StringVar(ws)
parent_variable.set(folders[0]) # default value
w = OptionMenu(ws, parent_variable, *folders, command=update_sub_folders)
w.place(x=400, y=50)

def upload_pdf():
    print(parent_variable.get())
    print(sub_variable.get())
    global current_filename
    global current_pagecount
    print(current_pagecount)
    for i in range(0, current_pagecount):
        fb.upload_file(current_filename+"/"+str(i)+".png",parent_variable.get()+"/"+sub_variable.get()+"/"+str(i)+".png")
        print(current_filename+"/"+str(i)+".png",parent_variable.get()+"/"+sub_variable.get()+"/"+str(i)+".png")
    return

def display_images(filename):
    #create frame to display images
    imageframe = Frame(ws)
    imageframe.place(x=20, y=350)
    #get number of pages to display
    dir_path = program_path+"/"+filename
    count = 0
    # Iterate directory
    for path in os.listdir(dir_path):
        # check if current path is a file
        if os.path.isfile(os.path.join(dir_path, path)):
            count += 1
    print(count)
    for i in range(0, count):
        image1 = Image.open(dir_path+"/"+str(i)+".png")
        image1 = image1.resize((100,int(100*1.414)), Image.ANTIALIAS)
        test = ImageTk.PhotoImage(image1)
        label1 = Label(imageframe, image=test)
        label1.image = test
        # Position image
        label1.pack(side=LEFT)
    w = Label(ws, text="Document Preview")
    w.place(x=30, y=320)
    w=Button(ws, text="Upload PDF",command=upload_pdf)
    w.place(x=200, y=315)


    






ws.mainloop()