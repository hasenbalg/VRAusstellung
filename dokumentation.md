# Dokumentation
## VR/AR Wintersemester 2017/18
Prof. Dr.-Ing. Michael Teistler, HS Flensburg


571087 Frank Hasenbalg

## Grundidee
Es soll eine Ausstellung mit VR-Techniken mit Unity erstellt werden.

Um auswaehlen zu koennen, welche Ausstellungsstuecke in der VR-Ausstellung ausgestellt werden,
benoetigt man ein Werkzeug. Kuratoren koennen damit Austellungen erstellen
und die Ausstellungsflaeche anpassen.

Eine Ausstellung besteht aus Raumen, in denen jeweils ein Ausstellungsstueck steht. Dieses Ausstellungsstueck kann entweder
ein Video, ein Audio-File, ein Text, ein Bild oder ein 3D-Objekt sein.
Jedes Ausstellungsstueck kann mit einem Titel und einer Beschreibung versehen werden.

Die Ausstellungsraeume sind in einem Raster angeordnet, wobei ein Raum den Eingang in die Ausstellung darstellt.
Der Besucher wechselt die Raeume, in dem er, analog zur Realitaet, Tueren benutzt.


## Backend
Das Backend wurde mit WPF realisiert und ist unabhaengig von der VR-Anwendung. Es ermoeglicht
neue Ausstellungen anzulegen und bereits estellte Ausstellungen zu veraendern.
Das Backend ist in 2 grosse Bereiche gegliedert:

### Allgemeine Eingenschaften der Ausstellung
Hier koennen Titel, Beschreibung und Groesse der Ausstellung/ Anzahl der Ausstellungsstuecke festgelegt werden.
Im gleichen Bereich koennen auch die Farben der kompletten Ausstellungsumgebung festgelegt werden.

Die Titel und der Beschreibung der Ausstellung werden im Eingang der Ausstellung angezeigt.

### Eigenschaften der Ausstellungsstuecke
Hier werden linker Hand alle Stellplaetze fuer Ausstellungsstuecke in einem Raster mit Namen angezeigt. Die Anordnung laesst sich per Drag'n'Drop aendern.
Klickt man bei einem Ausstellungsstueck im Raster auf den Aenderungs-Button, werden rechter Hand dessen Eigenschaften editierbar angzeigt.
Bei der Festlegung des Types des Ausstellungsstuecks kann zwischen Video, Audio, Text, Bild und 3D-Objekt gewaehlt werden.

## Frontend
Der Besucher der Ausstellung kann sich in den Ausstellungsraeumen frei bewegen und die Ausstellungsstuecke anschauen.
Die Ausstellungsraeume sind sind bewusst abstrakt und simpel gehalten, um moeglichst wenig vom Ausstellungsstueck abzulenken. Aehnlich simpel soll auch die Bedienung nicht vom Ausstellungsstueck ablenken.

### Bedienbarkeit
Die Bedienung wurde auf einen HTC Vive Controller beschraenkt.
- Die *Zeigerichtung* des Controllers waehlt den Raum aus.
- *Trigger* wechselt den Raum.
- *Touchpad* blendet Infotexte zu den Ausstellungsstuecken ein, wenn das das Ausstellungsstueck selbst kein text ist.
- *Druck auf das Touchpad* scrollte den Text.
- *Grip-Buttons* zeigen eine Uebersicht der Raume an, auf der auch ein Positionsmarker ist.
- Die *Position* des Controllers in der Zeitleiste eines Videos oder eines Audio-Tracks ermoeglicht das Vor- und Zurueckspringen in der Wiedergabe.

## Entscheidungen nach der Version vom 13 Dez
- Einer der Raume bietet Platz fuer die Metadaten der Ausstellung. Sonst gibt es keine sinnvolle Moeglichkeit, diese unterzubringen.
- Die Namen der Ausstellungsstuecke wurden ueber die Tuerrahmen geschrieben, um die Uebersichtbarkeit zu verbessern.
- Die Texte zu den Austellungsstuecken werden eingeblendet, wenn der Daumen auf dem Touchpad liegt. So kann der Text einfach ein- und ausgeblendet werden.
- Die Text lassen sich ueber Druck auf dem Touchpad scrollen.
- Lichter ueber den Tueren waehlen den naechsten Raum aus.
- Medien, auf denen die Medien dargestellt werden, wurden gestrichen.

## Usertests
|Alias|Datum|Aufgabenstellung|Ziel erreicht|Anmerkungen|
|-----|-----|----------------|-------------|-----------|
|Robin|20.12|Finde den Raum hinten rechts|Ja||
|...|...|...|...|...|
