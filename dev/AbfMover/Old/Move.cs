﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbfMover.Old
{
    internal class Move
    {

        private static void MoveCellsIntoSubfolders()
        {

        }

        private static void CreateFolders(string[] filePaths, bool dryRun)
        {
            string[] newFolderPaths = filePaths
                .Select(x => GetTargetFolder(x))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToArray();

            foreach (string folderPath in newFolderPaths)
            {
                Console.WriteLine($"Creating: {folderPath}/");
                if (!dryRun && !Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string expFilePath = Path.Combine(folderPath, "experiment.txt");
                string folderDate = Path.GetFileName(folderPath);
                string expContent = $"Example notes from {folderDate}...";
                Console.WriteLine($"Writing: {expFilePath}/");
                if (!dryRun && !File.Exists(expFilePath))
                {
                    File.WriteAllText(expFilePath, expContent);
                }
            }
        }

        private static void MoveFiles(string[] filePaths, bool dryRun)
        {
            foreach (string filePath in filePaths)
            {
                string targetFolder = GetTargetFolder(filePath);
                if (string.IsNullOrEmpty(targetFolder))
                    continue;
                string targetFilePath = Path.Combine(targetFolder, Path.GetFileName(filePath));
                string targetFolderName = Path.GetFileName(targetFolder);
                string fileName = Path.GetFileName(filePath);
                Console.WriteLine($"Moving: {filePath} -> {targetFolderName}/{fileName}");
                if (dryRun)
                    continue;
                File.Move(filePath, targetFilePath);
            }
        }

        private static void UpdateCellsFile(string path, bool dryRun)
        {
            string cellsFilePath = Path.Combine(path, "cells.txt");
            if (!File.Exists(cellsFilePath))
                return;
            string[] lines = File.ReadAllLines(cellsFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string firstPart = lines[i].Split(" ")[0];
                if (firstPart.Contains("/"))
                    continue;
                string targetFolder = GetTargetFolder(firstPart + ".abf");
                if (string.IsNullOrEmpty(targetFolder))
                    continue;
                lines[i] = $"{targetFolder}/{lines[i]}";
                Console.WriteLine($"Cell: {lines[i]}");
            }
            if (dryRun)
                return;
            Console.WriteLine($"Writing: {cellsFilePath}");
            File.WriteAllLines(cellsFilePath, lines);
        }

        public static string GetTargetFolder(string abfFilePath)
        {
            string filename = Path.GetFileName(abfFilePath);
            string directoryName = Path.GetDirectoryName(abfFilePath)!;

            string[] extensionsToMove = { ".abf", ".tif", ".sta", ".ignored" };
            bool isFileTypeToMove = extensionsToMove.Contains(Path.GetExtension(filename).ToLower());
            if (!isFileTypeToMove)
                return string.Empty;

            bool firstTwoCharsAreNumeric = int.TryParse(filename.Substring(0, 4), out _);
            if (!firstTwoCharsAreNumeric)
                return string.Empty;

            if (filename.Length == 8)
            {
                string folderName = filename.Substring(0, filename.Length - 7);
                return Path.Combine(directoryName, folderName);
            }
            else if (filename.Split("_").Length > 3)
            {
                string folderName = string.Join("-", filename.Split("_").Take(3));
                return Path.Combine(directoryName, folderName);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
