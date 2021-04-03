import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import ProfileStore from "./profileStore";
import UserStore from "./userStore";

interface Store {
    activityStore: ActivityStore;
    commonStore: CommonStore;
    userStore: UserStore;
    modalStore: ModalStore;
    profileStore: ProfileStore;
}

//a medida que criamos novas stores elas serão colocadas aqui nesse objeto
export const store: Store = {
    activityStore: new ActivityStore(),
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    modalStore: new ModalStore(),
    profileStore: new ProfileStore()
}

//isso aqui permite que a store fica disponível no contexto react
export const StoreContext = createContext(store);

//react hook simples que permite usar/disponibilizar as stores dentro dos componentes
export function useStore() {
    //isso daqui é para usar o contexto que criamos aqui
    return useContext(StoreContext);
}