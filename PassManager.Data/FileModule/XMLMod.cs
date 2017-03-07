using System;
using System.Xml.Linq;

namespace PassManager.Data.FileModule
{
	internal class XMLMod : IFileModule
	{
		private const string _STRING_ID = "PassManager";
		//private const float VERSION = 1.0f;

		public PassCollection OpenFile(string path)
		{
			try
			{
				XDocument xDoc = XDocument.Load(path);
				return xDoc.Root.Name == _STRING_ID ? _ParseToPassCollection(xDoc) : null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public bool SaveFile(string path, IDataModule passCollection)
		{
			if (passCollection == null)
				return false;

			try
			{
				XDocument xDoc = _ParseToXml(passCollection);
				xDoc?.Save(path);

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private PassCollection _ParseToPassCollection(XDocument xDoc)
		{
			PassCollection passCollection = new PassCollection();

			foreach (XElement element in xDoc.Root.Elements())
				passCollection.AddPassContainer(_ParseToPassContainer(element));

			return passCollection;
		}

		private PassContainer _ParseToPassContainer(XElement element)
		{
			PassContainer passContainer = new PassContainer();

			foreach (var property in typeof(PassContainer).GetProperties())
			{
				XElement xElement = element.Element(property.Name);
				if (xElement != null)
					property.SetValue(passContainer, xElement.Value);
			}

			return passContainer;
		}
		private XDocument _ParseToXml(IDataModule passCollection)
		{
			if (passCollection == null)
				return null;

			XDocument xDoc = new XDocument(new XElement(_STRING_ID));

			foreach (PassContainer passNote in passCollection)
			{
				XElement element = new XElement(nameof(passNote));

				foreach (var propertyInfo in typeof(PassContainer).GetProperties())
					element.Add(new XElement(propertyInfo.Name, propertyInfo.GetValue(passNote)));

				xDoc.Root.Add(element);
			}
			
			return xDoc;
		}
	}
}
