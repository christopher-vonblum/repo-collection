$(call PKG_INIT_BIN, 0.9.10)
$(PKG)_SOURCE:=mcabber-$($(PKG)_VERSION).tar.bz2
$(PKG)_HASH:=431e47ba3095c1cc6c11931bab9034c42ba0911dfadcc137ba765803e4460cb5
$(PKG)_SITE:=http://mirror.mcabber.com/files

$(PKG)_PATCH_POST_CMDS += $(call PKG_ADD_EXTRA_FLAGS,LDFLAGS|LIBS,src)

$(PKG)_BINARY:=$($(PKG)_DIR)/src/mcabber
$(PKG)_TARGET_BINARY:=$($(PKG)_DEST_DIR)/usr/bin/mcabber

$(PKG)_DEPENDS_ON += glib2 ncurses

$(PKG)_REBUILD_SUBOPTS += FREETZ_PACKAGE_MCABBER_STATIC
$(PKG)_REBUILD_SUBOPTS += FREETZ_PACKAGE_MCABBER_WITH_SSL

ifeq ($(strip $(FREETZ_PACKAGE_MCABBER_WITH_SSL)),y)
$(PKG)_REBUILD_SUBOPTS += FREETZ_OPENSSL_SHLIB_VERSION
$(PKG)_DEPENDS_ON += openssl
endif

ifeq ($(strip $(FREETZ_PACKAGE_MCABBER_STATIC)),y)
$(PKG)_LDFLAGS := -static
ifeq ($(strip $(FREETZ_PACKAGE_MCABBER_WITH_SSL)),y)
$(PKG)_STATIC_LIBS += $(OPENSSL_LIBCRYPTO_EXTRA_LIBS)
endif
endif

$(PKG)_CONFIGURE_OPTIONS += $(if $(FREETZ_PACKAGE_MCABBER_WITH_SSL),--with-openssl,--without-ssl)
$(PKG)_CONFIGURE_OPTIONS += --disable-gpgme

$(PKG_SOURCE_DOWNLOAD)
$(PKG_UNPACKED)
$(PKG_CONFIGURED_CONFIGURE)

$($(PKG)_BINARY): $($(PKG)_DIR)/.configured
	$(SUBMAKE) -C $(MCABBER_DIR) \
		EXTRA_LDFLAGS="$(MCABBER_LDFLAGS)" \
		EXTRA_LIBS="$(MCABBER_STATIC_LIBS)"

$($(PKG)_TARGET_BINARY): $($(PKG)_BINARY)
	$(INSTALL_BINARY_STRIP)

$(pkg):

$(pkg)-precompiled: $($(PKG)_TARGET_BINARY)

$(pkg)-clean:
	-$(SUBMAKE) -C $(MCABBER_DIR) clean

$(pkg)-uninstall:
	$(RM) $(MCABBER_TARGET_BINARY)

$(PKG_FINISH)
