<template>
  <div id="root">
    <div :class="cssClasses">
      <RouterView
          name="layout"
          :title="title"
          :is-x-small="screen.isXSmall"
          :is-large="screen.isLarge"
      >
        <div class="content">
          <RouterView name="content"></RouterView>
        </div>
        <template #footer>
          <Footer/>
        </template>
      </RouterView>
    </div>
  </div>
</template>

<script>
import Footer from './components/static/Footer'

import {sizes, subscribe, unsubscribe} from "./utils/media-query";
import {mapState} from "vuex";

function getScreenSizeInfo() {
  const screenSizes = sizes();

  return {
    isXSmall: screenSizes["screen-x-small"],
    isLarge: screenSizes["screen-large"],
    cssClasses: Object.keys(screenSizes).filter(cl => screenSizes[cl])
  };
}

export default {
  name: 'app',
  data() {
    return {
      title: this.$appInfo.title,
      navTitle: this.$appInfo.navTitle,
      screen: getScreenSizeInfo(),
    };
  },
  computed: {
    // ...mapState(['currentUser']),
    cssClasses() {
      return ["app"].concat(this.screen.cssClasses);
    }
  },
  methods: {
    screenSizeChanged() {
      this.screen = getScreenSizeInfo();
    }
  },

  mounted() {
    subscribe(this.screenSizeChanged);
  },

  beforeDestroy() {
    unsubscribe(this.screenSizeChanged);
  },

  created() {
    // this.$store.dispatch('INIT_CURRENT_USER')
  },
  components: {
    Footer
  }
}
</script>

<style lang="scss">
html,
body {
  margin: 0;
  min-height: 100%;
  height: 100%;
}

#root {
  height: 100%;
}

* {
  box-sizing: border-box;
}

.app {
@import "./themes/generated/variables.base.scss";
  background-color: darken($base-bg, 5);
  display: flex;
  height: 100%;
  width: 100%;
}
</style>
