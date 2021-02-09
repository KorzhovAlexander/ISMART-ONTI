<template>
  <header class="header-component">
    <DxToolbar class="header-toolbar">
      <DxItem
          locate-in-menu="auto"
          location="before"
      >
        <DxButton
            icon="home"
            styling-mode="text"
            text="Главная"
            slot-scope="_"
            @click="()=> $router.push('/home')"
        />
      </DxItem>

      <DxItem
          locate-in-menu="auto"
          location="before"
      >
        <DxButton
            icon="find"
            styling-mode="text"
            text="Отследить заказ"
            slot-scope="_"
            @click="findOrder"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="before"
      >
        <DxButton
            v-if="!hasManagerRole()"
            icon="email"
            styling-mode="text"
            text="Обратная связь"
            slot-scope="_"
            @click="sendFeedBack"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="before"
      >
        <DxButton
            v-if="user && hasManagerRole()"
            icon="email"
            styling-mode="text"
            text="Обратная связь клиентов"
            slot-scope="_"
            @click="()=> $router.push('/feedback')"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="before"
      >
        <DxButton
            v-if="user && hasManagerRole()"
            styling-mode="text"
            text="Виды услуг"
            slot-scope="_"
            @click="openServiceTypeForm"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="before"
      >
        <DxButton
            v-if="user && hasManagerRole()"
            styling-mode="text"
            text="Единицы измерения"
            slot-scope="_"
            @click="openUnitForm"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="after"
      >
        <DxButton
            v-if="user && hasUserRole()"
            icon="cart"
            styling-mode="text"
            text="Мои заказы"
            slot-scope="_"
            @click="()=> $router.push('/cart')"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="after"
      >
        <DxButton
            v-if="user && hasCourierRole()"
            icon="cart"
            styling-mode="text"
            text="Доступные заказы"
            slot-scope="_"
            @click="()=> $router.push('/cart')"
        />
      </DxItem>
      <DxItem
          locate-in-menu="auto"
          location="after"
      >
        <DxButton
            v-if="user && hasManagerRole()"
            icon="cart"
            styling-mode="text"
            text="Все заказы"
            slot-scope="_"
            @click="()=> $router.push('/cart')"
        />
      </DxItem>
      <DxItem
          location="after"
          locate-in-menu="auto"
      >
        <DxButton
            v-if="!user"
            icon="user"
            class="user-button authorization"
            height="100%"
            styling-mode="outlined"
            text="Войти"
            @click="logIn($event)"
            slot-scope="_"
        />
        <DxButton
            v-else
            icon="user"
            class="user-button authorization"
            height="100%"
            styling-mode="outlined"
            :text="`Выйти ${user.userName}`"
            @click="logOut($event)"
            slot-scope="_"
        />
      </DxItem>
    </DxToolbar>
    <OrderFindForm
        :visible.sync="orderFindFormVisible"
    />
    <FeedbackForm
        :visible.sync="feedbackFormVisible"
        @submit="feedBackSubmit"
    />
    <ServiceTypeForm
        :visible.sync="serviceTypeFormVisible"
    />
    <UnitForm
      :visible.sync="unitFormVisible"
    />
  </header>
</template>

<script>
import DxButton from "devextreme-vue/button";
import DxToolbar, {DxItem} from "devextreme-vue/toolbar";
import DxScrollView from "devextreme-vue/scroll-view";
import auth from "../../auth";
import OrderFindForm from "../../components/order-find-form";
import {mapState} from 'vuex';
import FeedbackForm from "../feedback-form";
import notify from "devextreme/ui/notify";
import ServiceTypeForm from '../service-type-form'
import UnitForm from '../unit-form'

export default {
  name: "NavBar",
  props: {
    title: String,
  },

  data() {
    return {
      user: null,
      orderFindFormVisible: false,
      feedbackFormVisible: false,
      serviceTypeFormVisible: false,
      unitFormVisible: false,
    }
  },
  created() {
    // this.$store.dispatch('INIT_CURRENT_USER')
    auth.getUser().then((e) => this.user = e.data);
  },
  methods: {
    findOrder() {
      this.orderFindFormVisible = true
    },
    sendFeedBack() {
      this.feedbackFormVisible = true
    },
    openServiceTypeForm() {
      this.serviceTypeFormVisible = true;
    },
    openUnitForm() {
      this.unitFormVisible = true;
    },
    async feedBackSubmit(data) {
      console.log(data)
      let self = this;
      return fetch('api/feedback/', {
        method: "POST",
        credentials: 'include',
        headers: {
          'Accept': 'application/json, text/plain, */*',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
      })
          .then((e) => {
            if (e.ok) {
              self.feedbackFormVisible = false;
              notify('Спасибо за вашу обратную связь', "success", 2000);
              return e.data;
            }
            notify('Ошибка отправки', "error", 2000);
          }).catch(c => {
            console.log(c)
            return {
              isOk: false,
              message: "Ошибка"
            };
          });
    },
    async logIn(e) {
      console.log('login click')
      await this.$router.push({
        path: "/login-form",
        query: {redirect: this.$route.path}
      });
    },
    async logOut(e) {
      auth.logOut();
      await this.$router.push({
        path: "/login-form",
        query: {redirect: this.$route.path}
      });
    },
    hasCourierRole() {
      return auth.hasCourierRole()
    },
    hasManagerRole() {
      return auth.hasManagerRole()
    },
    hasUserRole() {
      return auth.hasUserRole()
    },
  },
  computed: {
    // ...mapState(['currentUser',]),
  },
  components: {
    FeedbackForm,
    DxToolbar,
    DxItem,
    DxScrollView,
    DxButton,
    OrderFindForm,
    ServiceTypeForm,
    UnitForm
  }
}
</script>

<style lang="scss">
@import "../../themes/generated/variables.base.scss";
@import "../../dx-styles.scss";

.header-component {
  flex: 0 0 auto;
  z-index: 1;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.12), 0 1px 2px rgba(0, 0, 0, 0.24);

  .dx-toolbar .dx-toolbar-item.menu-button > .dx-toolbar-item-content .dx-icon {
    color: $base-accent;
  }
}

.dx-toolbar.header-toolbar .dx-toolbar-items-container .dx-toolbar-after {
  padding: 0 40px;

  .screen-x-small & {
    padding: 0 20px;
  }
}

.dx-toolbar .dx-toolbar-item.dx-toolbar-button.menu-button {
  width: $side-panel-min-width;
  text-align: center;
  padding: 0;
}

.header-title .DxItem-content {
  padding: 0;
  margin: 0;
}

.dx-theme-generic {
  .dx-toolbar {
    padding: 10px 0;
  }

  .user-button > .dx-button-content {
    //padding: 3px;
  }
}
</style>