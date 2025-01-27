#!/bin/sh

. /usr/lib/libmodcgi.sh

ALTWIDTH="$(modconf value RRDSTATS_ALTWIDTH rrdstats)"
[ $ALTWIDTH -gt 500 ] 2>/dev/null && ALTWIDTH=$(( $ALTWIDTH - 350 )) || ALTWIDTH=500
cgi --width=$ALTWIDTH
cgi_begin "RRDstats"

source /usr/lib/cgi-bin/rrdstats/stats.cgi

cgi_end
