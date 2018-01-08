set WorkDir=%1\
set BackupDir=%~dp0Backup

#echo %WorkDir%\CmsWeb\www\Custom\Components\DataView\DomainSearch\EmbedDomainSearch.ascx %BackupDir%\EmbedDomainSearch.ascx
#exit

mkdir %BackupDir%
#copy %WorkDir%CmsWeb\www\search.js %~dp0Backup\search.js
copy %WorkDir%CmsWeb\www\Custom\Components\DataView\DomainSearch\EmbedDomainSearch.ascx %BackupDir%\EmbedDomainSearch.ascx

copy search.js %WorkDir%CmsWeb\www
copy EmbedDomainSearch.ascx %WorkDir%CmsWeb\www\Custom\Components\DataView\DomainSearch
