set UNITY_VERSION=2021.3.5f1
 
c:
set JAVA_HOME=c:\Program Files\Unity\Hub\Editor\2021.3.5f1\Editor\Data\PlaybackEngines\AndroidPlayer\OpenJDK\
set ANDROID_HOME=c:\Program Files\Unity\Hub\Editor\2021.3.5f1\Editor\Data\PlaybackEngines\AndroidPlayer
cd c:\Program Files\Unity\Hub\Editor\2021.3.5f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\tools\bin\
echo.> samag\.android\repositories.cfg
 
cmd /C sdkmanager --update
cmd /C sdkmanager "platform-tools" "platforms;android-28"