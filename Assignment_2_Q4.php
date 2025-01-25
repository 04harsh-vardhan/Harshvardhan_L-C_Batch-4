class Book {
 
 function getTitle() {
     return "A Great Book";
 }

 function getAuthor() {
     return "John Doe";
 }
}

class Reader implements Book{
function turnPage() {
     // pointer to next page
 }

 function getCurrentPage() {
     return "current page content";
 }
}

class Librarian implements Book{
function getLocation() {
     // returns the position in the library
     // ie. shelf number & room number
 }

 function save() {
     $filename = '/documents/'. $this->getTitle(). ' - ' . $this->getAuthor();
     file_put_contents($filename, serialize($this));
 }
}

interface Printer {

 function printPage($page);
}

class PlainTextPrinter implements Printer {

 function printPage($page) {
     echo $page;
 }

}

class HtmlPrinter implements Printer {

 function printPage($page) {
     echo '<div style="single-page">' . $page . '</div>';
 }
}
