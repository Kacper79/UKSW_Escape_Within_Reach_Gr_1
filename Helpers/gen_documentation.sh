curl https://github.com/Wyamio/Wyam/releases/download/v1.6.0/Wyam-v1.6.0.zip
unzip Wyam-v.1.6.0.zip -d wyam
cd wyam
mkdir src
git clone https://github.com/Kacper79/UKSW_Escape_Within_Reach_Gr_1
./Wyam new -r docs
./Wyam build
find . -type f -name "*.html" -exec sed -E -i 's|<a href="(/[^"]*[^/])"|<a href="/doc\1/index.html"|g' {} +
find . -type f -name "*.html" -exec sed -E -i 's|<link href="(/assets/[^"]+)"|<link href="/doc\1"|g' {} +
find . -type f -name "*.html" -exec sed -E -i 's|<script src="(/assets/[^"]+)"|<script src="/doc\1"|g' {} +
scp -r output DevSolfeed2@s1.ct8.pl:/home/DevSolfeed2/domains/devsolfeed2.ct8.pl/public_html/doc