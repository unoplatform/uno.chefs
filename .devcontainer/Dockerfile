# .NET 9 SDK — matches the net9.0-* target frameworks in Chefs.csproj and the
# Aspire 9.x AppHost. Do not bump to sdk:10.0 without also moving the repo's
# TFMs; the net9.0-android/-ios workloads are not carried by the 10.0 image.
FROM mcr.microsoft.com/dotnet/sdk:9.0

ARG TZ
ENV TZ="$TZ"

ARG CLAUDE_CODE_VERSION=latest
ARG COPILOT_CLI_VERSION=latest

# ---------------------------------------------------------------------------
# System packages
# ---------------------------------------------------------------------------
RUN apt-get update && apt-get install -y --no-install-recommends \
  less \
  git \
  procps \
  sudo \
  fzf \
  zsh \
  man-db \
  unzip \
  gnupg2 \
  jq \
  nano \
  vim \
  # DNS-based firewall (see init-firewall.sh)
  dnsmasq \
  iptables \
  iproute2 \
  dnsutils \
  # Uno / Skia desktop dependencies
  xvfb \
  fluxbox \
  libgtk-3-dev \
  libwebkit2gtk-4.1-dev \
  # SkiaSharp font resolution (LiveCharts + Mapsui render text via Skia)
  libfontconfig1 \
  # MediaPlayerElement on Linux/Skia is backed by LibVLC — Chefs plays recipe
  # videos on the RecipeDetails page, so this is not optional here.
  vlc \
  libvlc-dev \
  # File picker dialog support (xdg-desktop-portal chain + zenity fallback)
  xdg-desktop-portal \
  xdg-desktop-portal-gtk \
  # xdg-open binary (Process.Start(url) on Linux)
  xdg-utils \
  zenity \
  dbus \
  dbus-user-session \
  # Chefs.UITests drives WebAssembly through Selenium. Debian's chromium +
  # chromium-driver are version-matched by the distro, which avoids the
  # chromedriver/Chrome version skew the CI scripts work around by pinning
  # npm chromedriver@127 + puppeteer.
  chromium \
  chromium-driver \
  # SSH client (WSL host terminal)
  openssh-client \
  # Misc
  curl \
  wget \
  ca-certificates \
  && apt-get clean && rm -rf /var/lib/apt/lists/*

# ---------------------------------------------------------------------------
# GitHub CLI (not in bookworm defaults)
# ---------------------------------------------------------------------------
RUN curl -fsSL https://cli.github.com/packages/githubcli-archive-keyring.gpg \
      | dd of=/usr/share/keyrings/githubcli-archive-keyring.gpg \
  && chmod go+r /usr/share/keyrings/githubcli-archive-keyring.gpg \
  && echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/githubcli-archive-keyring.gpg] https://cli.github.com/packages stable main" \
      > /etc/apt/sources.list.d/github-cli.list \
  && apt-get update && apt-get install -y --no-install-recommends gh \
  && apt-get clean && rm -rf /var/lib/apt/lists/*

# ---------------------------------------------------------------------------
# Node.js 20 (Claude Code / Copilot CLI npm tooling, MCP servers via npx)
# ---------------------------------------------------------------------------
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
  && apt-get install -y --no-install-recommends nodejs \
  && apt-get clean && rm -rf /var/lib/apt/lists/*

# ---------------------------------------------------------------------------
# git-delta (better diffs)
# ---------------------------------------------------------------------------
ARG GIT_DELTA_VERSION=0.18.2
RUN ARCH=$(dpkg --print-architecture) && \
  wget -q "https://github.com/dandavison/delta/releases/download/${GIT_DELTA_VERSION}/git-delta_${GIT_DELTA_VERSION}_${ARCH}.deb" && \
  dpkg -i "git-delta_${GIT_DELTA_VERSION}_${ARCH}.deb" && \
  rm "git-delta_${GIT_DELTA_VERSION}_${ARCH}.deb"

# ---------------------------------------------------------------------------
# Non-root user: developer (uid 1000)
# Container is the security boundary, not in-container privileges.
# ---------------------------------------------------------------------------
ARG USERNAME=developer
RUN existing_user=$(getent passwd 1000 | cut -d: -f1 || true) && \
  if [ -n "$existing_user" ] && [ "$existing_user" != "${USERNAME}" ]; then \
    usermod -l ${USERNAME} -d /home/${USERNAME} -m "$existing_user" && \
    groupmod -n ${USERNAME} "$existing_user" 2>/dev/null || true; \
  elif [ -z "$existing_user" ]; then \
    useradd -m -s /bin/zsh -u 1000 ${USERNAME}; \
  fi && \
  chsh -s /bin/zsh ${USERNAME} && \
  echo "${USERNAME} ALL=(ALL) NOPASSWD:ALL" > /etc/sudoers.d/${USERNAME} && \
  chmod 0440 /etc/sudoers.d/${USERNAME}

# Persist shell history (env vars are set in the zsh-in-docker -a flag below)
RUN mkdir -p /commandhistory && \
  touch /commandhistory/.bash_history && \
  chown -R ${USERNAME} /commandhistory

ENV DEVCONTAINER=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1

# Ensure non-root user owns /usr/local/share for npm global installs
RUN mkdir -p /usr/local/share/npm-global && \
  chown -R ${USERNAME}:${USERNAME} /usr/local/share/npm-global

# Create workspace and config directories
RUN mkdir -p /uno-chefs /home/${USERNAME}/.claude /home/${USERNAME}/.copilot /home/${USERNAME}/.nuget && \
  chown -R ${USERNAME}:${USERNAME} /uno-chefs /home/${USERNAME}/.claude /home/${USERNAME}/.copilot /home/${USERNAME}/.nuget

WORKDIR /uno-chefs

# ---------------------------------------------------------------------------
# zsh + powerlevel10k
# ---------------------------------------------------------------------------
ARG ZSH_IN_DOCKER_VERSION=1.2.0
USER ${USERNAME}
RUN sh -c "$(wget -O- https://github.com/deluan/zsh-in-docker/releases/download/v${ZSH_IN_DOCKER_VERSION}/zsh-in-docker.sh)" -- \
  -p git \
  -p fzf \
  -a "source /usr/share/doc/fzf/examples/key-bindings.zsh 2>/dev/null || true" \
  -a "source /usr/share/doc/fzf/examples/completion.zsh 2>/dev/null || true" \
  -a "export PROMPT_COMMAND='history -a' && export HISTFILE=/commandhistory/.bash_history" \
  -x

ENV SHELL=/bin/zsh
ENV EDITOR=nano
ENV VISUAL=nano

# npm global prefix — writable by non-root user
ENV NPM_CONFIG_PREFIX=/usr/local/share/npm-global
ENV PATH=$PATH:/usr/local/share/npm-global/bin

# ---------------------------------------------------------------------------
# Claude Code CLI (native installer — installs to ~/.local/bin/claude)
# ---------------------------------------------------------------------------
RUN curl -fsSL https://claude.ai/install.sh | bash -s -- ${CLAUDE_CODE_VERSION}
ENV PATH="/home/${USERNAME}/.local/bin:${PATH}"

# ---------------------------------------------------------------------------
# GitHub Copilot CLI (installs to ~/.local/bin/copilot)
# Drop this layer (and the `copilot` aliases in post-start.sh, the
# github.copilot* VS Code extensions, and the githubcopilot.com /
# certificate-revocation entries in init-firewall.sh) if the team
# standardises on a single agent CLI.
# ---------------------------------------------------------------------------
RUN curl -fsSL https://gh.io/copilot-install | bash -s -- ${COPILOT_CLI_VERSION}

# ---------------------------------------------------------------------------
# Microsoft Build of OpenJDK 21 — required by the Android SDK tooling chain
# (sdkmanager, aapt2, d8) for `-f net9.0-android` builds. Installed manually
# because uno-check does not provision a JDK on Linux.
# ---------------------------------------------------------------------------
USER root
RUN . /etc/os-release && \
  if [ "$ID" = "ubuntu" ]; then \
    MS_REPO_PATH="ubuntu/${VERSION_ID}"; \
  else \
    MS_REPO_PATH="debian/${VERSION_ID%%.*}"; \
  fi && \
  wget -qO /tmp/packages-microsoft-prod.deb \
    "https://packages.microsoft.com/config/${MS_REPO_PATH}/packages-microsoft-prod.deb" && \
  dpkg -i /tmp/packages-microsoft-prod.deb && \
  rm /tmp/packages-microsoft-prod.deb && \
  apt-get update && apt-get install -y --no-install-recommends msopenjdk-21 && \
  apt-get clean && rm -rf /var/lib/apt/lists/* && \
  # msopenjdk-21 installs to /usr/lib/jvm/msopenjdk-21-<arch>. Resolve the real
  # directory by walking from `javac` and pin an arch-agnostic symlink, because
  # mcr.microsoft.com/dotnet/sdk:9.0 is multi-arch (this image may be built on
  # Apple Silicon).
  ln -sfn "$(dirname "$(dirname "$(readlink -f "$(command -v javac)")")")" \
    /usr/lib/jvm/msopenjdk-21-current

ENV JAVA_HOME=/usr/lib/jvm/msopenjdk-21-current
ENV PATH=${JAVA_HOME}/bin:${PATH}

# ---------------------------------------------------------------------------
# .NET workloads — wasm-tools for net9.0-browserwasm, android for
# net9.0-android. There is no emulator in this image (see .devcontainer/README);
# `android` is here so the TFM compiles, not so it can be deployed.
# ---------------------------------------------------------------------------
RUN dotnet workload install wasm-tools android

# ---------------------------------------------------------------------------
# uno-check — provisions the Android SDK (cmdline-tools, build-tools, platforms)
# into ANDROID_HOME and validates the workload set against the Uno.Sdk version
# in global.json. Pinned to the same version the repo's GitHub CI uses
# (.github/workflows/ci.yml) so the container and CI agree on what "valid"
# means.
#
# androidemulator is skipped deliberately: this container has no /dev/kvm, so a
# provisioned emulator + system image would be ~2GB of unusable download.
# openjdk is skipped because uno-check does not handle the JDK on Linux (it is
# installed above). `|| true` because uno-check exits non-zero for advisory
# findings that must not fail the image build.
# ---------------------------------------------------------------------------
ENV ANDROID_HOME=/opt/android-sdk
ENV ANDROID_SDK_ROOT=/opt/android-sdk
ENV PATH=${ANDROID_HOME}/platform-tools:${ANDROID_HOME}/cmdline-tools/latest/bin:${PATH}

USER ${USERNAME}
ENV PATH="${PATH}:/home/${USERNAME}/.dotnet/tools"
RUN dotnet tool install --global uno.check --version 1.28.3
USER root
RUN mkdir -p ${ANDROID_HOME} && chown -R ${USERNAME}:${USERNAME} ${ANDROID_HOME} && \
  uno-check -v --ci --non-interactive --fix \
    --skip xcode --skip gtk3 --skip vswin --skip vswinworkloads --skip unosdk \
    --skip dotnetnewunotemplates --skip vsmac --skip openjdk --skip androidemulator \
    --tfm net9.0-desktop --tfm net9.0-browserwasm --tfm net9.0-android \
    || true
RUN chown -R ${USERNAME}:${USERNAME} ${ANDROID_HOME}

# ---------------------------------------------------------------------------
# Chefs.UITests (Browser platform) — Uno.UITest.Selenium resolves the driver
# from a *directory* and the browser from a *binary path*. Debian installs
# chromedriver at /usr/bin/chromedriver, so the driver directory is /usr/bin.
# UNO_UITEST_PLATFORM / UNO_UITEST_TARGETURI stay unset: they are per-run
# choices, see .devcontainer/README.md.
# ---------------------------------------------------------------------------
ENV UNO_UITEST_DRIVERPATH_CHROME=/usr/bin
ENV UNO_UITEST_CHROME_BINARY_PATH=/usr/bin/chromium

# ---------------------------------------------------------------------------
# Neutralise VS Code git-credential forwarding
# dpkg-divert moves the real binary to /usr/bin/git.distrib so that even
# direct /usr/bin/git calls go through the wrapper.
# ---------------------------------------------------------------------------
RUN dpkg-divert --rename --add /usr/bin/git && \
  printf '#!/bin/sh\nunset GIT_ASKPASS VSCODE_GIT_ASKPASS_MAIN VSCODE_GIT_ASKPASS_NODE VSCODE_GIT_IPC_HANDLE SSH_AUTH_SOCK\nexec /usr/bin/git.distrib -c credential.helper= "$@"\n' \
  > /usr/bin/git && \
  chmod +x /usr/bin/git

# ---------------------------------------------------------------------------
# Helper scripts
# ---------------------------------------------------------------------------
COPY init-firewall.sh /usr/local/bin/
COPY ssh-wsl-host.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/init-firewall.sh /usr/local/bin/ssh-wsl-host.sh && \
  echo "${USERNAME} ALL=(root) NOPASSWD: /usr/local/bin/init-firewall.sh" > /etc/sudoers.d/${USERNAME}-firewall && \
  chmod 0440 /etc/sudoers.d/${USERNAME}-firewall

# ---------------------------------------------------------------------------
# Disable Claude Code adaptive thinking
# ---------------------------------------------------------------------------
ENV CLAUDE_CODE_DISABLE_ADAPTIVE_THINKING=1

USER ${USERNAME}
