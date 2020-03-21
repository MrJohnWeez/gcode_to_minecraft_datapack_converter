using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightBuzz.Archiver;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.IO;

public static class Archive
{
	/// <summary>
	/// Compress a folder or file into a zip file
	/// </summary>
	/// <param name="source">File path of folder or file</param>
	/// <param name="destination">Folder path of output with zip name</param>
	/// <returns></returns>
	public static Task CompressAsync(string source, string destinationWithZipName, ProgressAmount<float> progess, CancellationToken cancellationToken, bool deleteAfterArchiving = false)
	{
		return Task.Run(() =>
		{
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				progess.ReportValue(0, "Compressing into .zip", "Compressing files (may take a while)");
				Archiver.Compress(source, destinationWithZipName);
				if(deleteAfterArchiving)
				{
					progess.ReportValue(0.7f, "Compressing into .zip", "Deleting temp files");

					if (Directory.Exists(source))
						Directory.Delete(source, true);
					else if (File.Exists(source))
						File.Delete(source);
				}
				progess.ReportValue(1, "Compressing into .zip", "Finished");
			}
			catch (OperationCanceledException wasCanceled)
			{
				throw wasCanceled;
			}
			catch (ObjectDisposedException wasAreadyCanceled)
			{
				throw wasAreadyCanceled;
			}
		});
	}

	/// <summary>
	/// Decompress a zip file into a folder or file
	/// </summary>
	/// <param name="source">File path of zip file</param>
	/// <param name="destination">Folder path of output with zip name</param>
	/// <returns></returns>
	public static Task DecompressAsync(string source, string destinationWithZipName, ProgressAmount<float> progess, CancellationToken cancellationToken, bool deleteAfterArchiving = false)
	{
		return Task.Run(() =>
		{
			try
			{
				cancellationToken.ThrowIfCancellationRequested();
				progess.ReportValue(0, "Decompressing files", "Decompressing files (may take a while)");
				Archiver.Decompress(source, destinationWithZipName);
				if (deleteAfterArchiving)
				{
					progess.ReportValue(0.7f, "Decompressing files", "Deleting temp files");

					if(Directory.Exists(source))
						Directory.Delete(source, true);
					else if(File.Exists(source))
						File.Delete(source);
				}
				progess.ReportValue(0, "Decompressing files", "Finished");
			}
			catch (OperationCanceledException wasCanceled)
			{
				throw wasCanceled;
			}
			catch (ObjectDisposedException wasAreadyCanceled)
			{
				throw wasAreadyCanceled;
			}
		});
	}

	/// <summary>
	/// Compress a folder or file into a zip file
	/// </summary>
	/// <param name="source">File path of folder or file</param>
	/// <param name="destination">Folder path of output with zip name</param>
	/// <returns></returns>
	public static void Compress(string source, string destinationWithZipName)
	{
		Archiver.Compress(source, destinationWithZipName);
	}

	/// <summary>
	/// Decompress a zip file into a folder or file
	/// </summary>
	/// <param name="source">File path of zip file</param>
	/// <param name="destination">Folder path of output with zip name</param>
	/// <returns></returns>
	public static void Decompress(string source, string destinationWithZipName)
	{
		Archiver.Decompress(source, destinationWithZipName);
	}
}
