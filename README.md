
# My Journalist

It is an application that allows you to store and organize your entries in a systematic way and automatically group them by month and send summary emails.

## Current status

The principle of the application is to read the content from the myNotes.txt in periods set by the user, starting from the selected hour, next write it to the Records_from_last_24_h.json file as an record, then merge all the entries from one day and save them to the RecordBook_(month name).json archive file. While it's running, aplication observes a txt file and if the size of the file changes, the contents will be read and txt file will be cleared, next adds a new record to the Records_from_last_24_h.json file. The third .json file that is created is a list of the tags that appeared in the content of the .txt file.

## Roadmap

- Add logging

- Add some async methods
