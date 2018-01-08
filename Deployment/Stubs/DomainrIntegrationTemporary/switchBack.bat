set WorkDir=%1\
set BackupDir=%~dp0Backup\

#copy %BackupDir%\search.js %WorkDir%CmsWeb\www
copy %BackupDir%EmbedDomainSearch.ascx %WorkDir%CmsWeb\www\Custom\Components\DataView\DomainSearch
