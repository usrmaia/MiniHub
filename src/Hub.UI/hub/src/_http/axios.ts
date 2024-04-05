'use client'

import axios from 'axios';

import env from '@/env';
import { getAuthToken } from '@/_services';

const AxiosCreate = () => {
  const token = getAuthToken();

  return axios.create({
    baseURL: env.API_URL,
    timeout: 5000,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token ? token.accessToken : ''}`,
    },
  });
};

export const Axios = AxiosCreate();

Axios.interceptors.request.use(
  config => config,
  error => Promise.reject(error)
);

Axios.interceptors.response.use(
  response => {
    console.debug('API/UI Response:', response);
    return response;
  },
  error => {
    if (error.response) {
      console.debug('API/UI Response Error:', error.response.data);
      console.debug('API/UI Status:', error.response.status);
    } else if (error.request) {
      console.debug('API/UI Request Error:', error.request);
    } else {
      console.debug('API/UI Error:', error.message);
    }
    return Promise.reject(error);
  }
);
