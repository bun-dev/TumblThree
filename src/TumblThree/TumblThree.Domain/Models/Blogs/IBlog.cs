﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TumblThree.Domain.Models
{
    public interface IBlog : INotifyPropertyChanged
    {
        string Name { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        string Url { get; set; }

        string Location { get; set; }

        string ChildId { get; set; }

        ulong LastId { get; set; }

        string Tags { get; set; }

        int Rating { get; set; }

        int Progress { get; set; }

        int Posts { get; set; }

        int Texts { get; set; }

        int Answers { get; set; }

        int Quotes { get; set; }

        int Photos { get; set; }

        int NumberOfLinks { get; set; }

        int Conversations { get; set; }

        int Videos { get; set; }

        int Audios { get; set; }

        int PhotoMetas { get; set; }

        int VideoMetas { get; set; }

        int AudioMetas { get; set; }

        int DownloadedTexts { get; set; }

        int DownloadedQuotes { get; set; }

        int DownloadedPhotos { get; set; }

        int DownloadedLinks { get; set; }

        int DownloadedAnswers { get; set; }

        int DownloadedConversations { get; set; }

        int DownloadedVideos { get; set; }

        int DownloadedAudios { get; set; }

        int DownloadedPhotoMetas { get; set; }

        int DownloadedVideoMetas { get; set; }

        int DownloadedAudioMetas { get; set; }

        int DownloadedImages { get; set; }

        int TotalCount { get; set; }

        bool DownloadAudio { get; set; }

        bool DownloadPhoto { get; set; }

        bool DownloadVideo { get; set; }

        bool DownloadText { get; set; }

        bool DownloadAnswer { get; set; }

        bool DownloadConversation { get; set; }

        bool DownloadLink { get; set; }

        bool DownloadQuote { get; set; }

        bool CreatePhotoMeta { get; set; }

        bool CreateVideoMeta { get; set; }

        bool CreateAudioMeta { get; set; }

        MetadataType MetadataFormat { get; set; }

        bool DumpCrawlerData { get; set; }

        string FileDownloadLocation { get; set; }

        string DownloadPages { get; set; }

        int PageSize { get; set; }

        string DownloadFrom { get; set; }

        string DownloadTo { get; set; }

        string Password { get; set; }

        bool DownloadRebloggedPosts { get; set; }

        bool SkipGif { get; set; }

        bool ForceSize { get; set; }

        bool ForceRescan { get; set; }

        int DuplicatePhotos { get; set; }

        int DuplicateVideos { get; set; }

        int DuplicateAudios { get; set; }

        string Notes { get; set; }

        bool DownloadUrlList { get; set; }

        string LastDownloadedPhoto { get; set; }

        string LastDownloadedVideo { get; set; }

        bool DownloadGfycat { get; set; }

        GfycatTypes GfycatType { get; set; }

        bool DownloadImgur { get; set; }

        bool DownloadWebmshare { get; set; }

        WebmshareTypes WebmshareType { get; set; }

        bool DownloadMixtape { get; set; }

        MixtapeTypes MixtapeType { get; set; }

	    bool DownloadMega { get; set; }

	    MegaTypes MegaType { get; set; }

	    bool DownloadGoogleDrive { get; set; }

	    GoogleDriveTypes GoogleDriveType { get; set; }
		
	    bool DownloadUguu { get; set; }

	    UguuTypes UguuType { get; set; }
		
	    bool DownloadSafeMoe { get; set; }

	    SafeMoeTypes SafeMoeType { get; set; }
		
	    bool DownloadLoliSafe { get; set; }

	    LoliSafeTypes LoliSafeType { get; set; }
		
	    bool DownloadCatBox { get; set; }

	    CatBoxTypes CatBoxType { get; set; }

        BlogTypes BlogType { get; set; }

        DateTime DateAdded { get; set; }

        DateTime LastCompleteCrawl { get; set; }

        int SettingsTabIndex { get; set; }

        bool Online { get; set; }

        bool CheckDirectoryForFiles { get; set; }

        bool Dirty { get; set; }

        Exception LoadError { get; set; }

        List<string> Links { get; }

        void UpdateProgress();

        void UpdatePostCount(string propertyName);

        void AddFileToDb(string fileName);

        bool CreateDataFolder();

        bool CheckIfFileExistsInDB(string url);

        bool CheckIfBlogShouldCheckDirectory(string url);

        bool CheckIfFileExistsInDirectory(string url);

        bool Save();

        IBlog Load(string fileLocation);

        string DownloadLocation();
    }
}
