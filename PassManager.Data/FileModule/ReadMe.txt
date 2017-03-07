Чтобы добавить поддержку сохранения/загрузки в кастомный формат, нужно:

- добавить класс реализующий интерфейс IFileModule;
- прописать новый тип в FileType.cs
- в параметризированном конструкторе FileModule добавить кейс с новым типом, где создается объект нового класса.

Например, мы хотим добывить поддержку сохранения/загрузки в файл тхт:

public FileModule(FileType fileType)
{
    switch (fileType)
    {
        case FileType.xml:
         FileInterface = new XMLMod();
             break;

    //Добавляем новый кейс
    case FileType.txt:
         FileInterface = new TXTMod();    //TXTMod - наш класс реализующий IFileModule
         break;
    }
}