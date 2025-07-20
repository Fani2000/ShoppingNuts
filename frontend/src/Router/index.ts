import {createRouter, createWebHashHistory} from 'vue-router'
import HomeView from '../View/HomeView.vue'
import Confirmation from "../View/ConfirmationView.vue";
import DetailsView from "../View/DetailsView.vue";
import ContactUsView from "../View/ContactUsView.vue";
import OrderDetails from "../View/OrderDetails.vue";
import AddToCartView from "../View/AddToCartView.vue";

const router = createRouter({
    history: createWebHashHistory(),
    routes: [
        {
            component: HomeView,
            path: '/',
            name: 'home'
        },
        {
            component: DetailsView,
            path: '/products/:id',
            name: 'product'
        },
        {
            component: OrderDetails,
            path: '/order-details/:id',
            name: 'details'
        },
        {
            component: AddToCartView,
            path: '/add-to-cart',
            name: 'add-to-cart'
        },
        {
            component: ContactUsView,
            path: '/contact',
            name: 'contact-us'
        },
        {
            component: Confirmation,
            path: '/confirmation',
            name: 'confirmation'
        }
    ]
})

export default router;