using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.ComponentModel;

public class CSVReader
{
    public Dictionary<string, T> LoadToCSVData<T>() where T : Data
    {
        Type type = typeof(T); //타입을 받아온다 (매개변수로 들어간 클래스명, == 파일명이 될것)_

        Dictionary<string, T> list = new Dictionary<string, T>();
        //파일이 경로에 있는지 없는지 검사
        if (!File.Exists($"{Application.dataPath}/@Resources/Data/Excel/{type.Name}.csv"))
        {
            return null;
        }
        //파일을 전부 읽어서
        StreamReader sr = new StreamReader($"{Application.dataPath}/@Resources/Data/Excel/{type.Name}.csv");
        string source = sr.ReadToEnd();
        sr.Close();

        //행마다 나눠서 리스트에 저장
        string[] lines = Regex.Split(source, "\n");

        //예외처리
        if (lines.Length <= 0) return null;

        //0번째 행은 헤더로 분리
        string[] propertyheader = Regex.Replace(lines[0], "\r", "").Split(",");

        // 데이터 파싱
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Replace(lines[i], "\r", "").Split(",");
            if (values.Length == 0 || string.IsNullOrEmpty(values[0])) continue;

            T entry = Activator.CreateInstance<T>();
            for (var j = 0; j < values.Length; j++)
            {
                PropertyInfo property = type.GetProperty(propertyheader[j]);
                if (property == null)
                {
                    Debug.LogError($"[DataTransformer] ParseData<{type.Name}>(): Data parsing failed. Property '{propertyheader[i]}' not found.");
                    return null;
                }
                property.SetValue(entry, ConvertValue(property.PropertyType, values[j]));

            }
            list.Add(values[0], entry);
        }

        return list;
    }
    private static object ConvertValue(Type type, string value)
    {
        // #1. 기본 값 자료인 경우 변환.
        TypeConverter converter = TypeDescriptor.GetConverter(type);
        if (converter != null && converter.CanConvertFrom(typeof(string))) return converter.ConvertFromString(value);

        // #2. 리스트 자료인 경우 변환.
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            Type itemType = type.GetGenericArguments()[0];
            IList list = Activator.CreateInstance(type) as IList;
            TypeConverter itemConverter = TypeDescriptor.GetConverter(itemType);
            if (itemConverter != null && itemConverter.CanConvertFrom(typeof(string)))
                foreach (var item in value.Split('|')) list.Add(itemConverter.ConvertFromString(item));
            else
                foreach (var item in value.Split('|')) list.Add(Activator.CreateInstance(itemType, item));
            return list;
        }
        return Activator.CreateInstance(type);
    }
}