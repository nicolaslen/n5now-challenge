import {AxiosInstance} from 'axios'
import MockAdapter from 'axios-mock-adapter'
/*import {mockHome} from '../../app/modules/home'
import {mockScoring} from '../../app/modules/scoring'*/

export default function mockAxios(axios: AxiosInstance) {
  const mock = new MockAdapter(axios, {delayResponse: 300})
  /*mockScoring(mock)
  mockHome(mock)*/
  return mock
}
