
# My Journalist

It is an application that allows you to store and organize your entries in a systematic way and automatically group them by month and send summary emails.

## Current status

The principle of the application is to read the content from the myNotes.txt file after startup and before closing, next write it to the Records_from_last_24_h.json file as an record, then merge all the entries from one day and save them to the RecordBook_(month name).json archive file. While it's running, aplication checks every 15 seconds to see if the content in the .txt file has appeared, and when it finds it, it adds a new record to the Records_from_last_24_h.json file. The third .json file that is created is a list of the tags that appeared in the content of the .txt file. Currently, the .txt file settings are in the TxtFileService class, and the .json file settings are in the JsonFileService class. All files are saved in a directory by default: "D:\temp".

## Roadmap

- Add logging

- Add sending email with summary

- Add reading .txt file when content is saved in

