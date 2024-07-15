#!/bin/bash
 
#rm DS_Store files first
find ./UnityFarmRPG/Assets/ -type f -name .DS_Store -exec rm {} \;

function purge-dir {

	while [ -n "$(find $1 -type d -empty)" ]
	do
			echo -ne "Found empty directories.\n Removing...\n\n";
			find $1 -type d -empty -exec rm -rf {}.meta \;
			find $1 -type d -empty -exec rm -rf {} \; &> /dev/null
	done
}

purge-dir "./UnityFarmRPG/Assets/"
purge-dir "./ExternalModules/"
 
echo -ne "\nClean.\n\nPress ENTER to exit.";
read