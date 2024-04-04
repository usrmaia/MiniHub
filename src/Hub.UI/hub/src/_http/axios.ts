'use client'

import axios from 'axios';

import env from '@/env';
import { getAuthToken } from '@/_services';

const AxiosCreate = () => {
  const accessToken = getAuthToken().accessToken;

  return axios.create({
    baseURL: env.API_URL,
    timeout: 5000,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${accessToken}`,
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
      console.error('API/UI Response Error:', error.response.data);
      console.error('API/UI Status:', error.response.status);
    } else if (error.request) {
      console.error('API/UI Request Error:', error.request);
    } else {
      console.error('API/UI Error:', error.message);
    }
    return Promise.reject(error);
  }
);
