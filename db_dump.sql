--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

-- Started on 2021-01-04 19:42:12

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 202 (class 1259 OID 16406)
-- Name: cards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cards (
    card_id character varying(50) NOT NULL,
    name character varying(50) NOT NULL,
    damage double precision NOT NULL,
    cat_id integer NOT NULL,
    user_id integer,
    package_id integer NOT NULL,
    element_id integer NOT NULL
);


ALTER TABLE public.cards OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16414)
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categories (
    cat_id integer NOT NULL,
    name character varying(50) NOT NULL
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 16444)
-- Name: elements; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.elements (
    element_id integer NOT NULL,
    name character varying(50) NOT NULL,
    effectiv character varying(50)
);


ALTER TABLE public.elements OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 16442)
-- Name: elements_element_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.elements_element_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.elements_element_id_seq OWNER TO postgres;

--
-- TOC entry 3022 (class 0 OID 0)
-- Dependencies: 204
-- Name: elements_element_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.elements_element_id_seq OWNED BY public.elements.element_id;


--
-- TOC entry 201 (class 1259 OID 16398)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    user_id integer NOT NULL,
    username character varying(50) NOT NULL,
    password character varying(50) NOT NULL,
    coins integer,
    active boolean
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 200 (class 1259 OID 16396)
-- Name: user_data_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_data_user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_data_user_id_seq OWNER TO postgres;

--
-- TOC entry 3023 (class 0 OID 0)
-- Dependencies: 200
-- Name: user_data_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_data_user_id_seq OWNED BY public.users.user_id;


--
-- TOC entry 2865 (class 2604 OID 16447)
-- Name: elements element_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.elements ALTER COLUMN element_id SET DEFAULT nextval('public.elements_element_id_seq'::regclass);


--
-- TOC entry 2864 (class 2604 OID 16401)
-- Name: users user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN user_id SET DEFAULT nextval('public.user_data_user_id_seq'::regclass);


--
-- TOC entry 3013 (class 0 OID 16406)
-- Dependencies: 202
-- Data for Name: cards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.cards (card_id, name, damage, cat_id, user_id, package_id, element_id) FROM stdin;
\.


--
-- TOC entry 3014 (class 0 OID 16414)
-- Dependencies: 203
-- Data for Name: categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.categories (cat_id, name) FROM stdin;
1	monster card
2	spell cards
\.


--
-- TOC entry 3016 (class 0 OID 16444)
-- Dependencies: 205
-- Data for Name: elements; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.elements (element_id, name, effectiv) FROM stdin;
1	normal	water
2	water	fire
3	fire	normal
\.


--
-- TOC entry 3012 (class 0 OID 16398)
-- Dependencies: 201
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (user_id, username, password, coins, active) FROM stdin;
11	altenhof	markus	20	f
10	altenho	markus	20	t
12	kienboec	daniel	20	t
13	kienbo	daniel	20	f
14	kienb	daniel	20	f
15	kien	daniel	20	f
\.


--
-- TOC entry 3024 (class 0 OID 0)
-- Dependencies: 204
-- Name: elements_element_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.elements_element_id_seq', 1, false);


--
-- TOC entry 3025 (class 0 OID 0)
-- Dependencies: 200
-- Name: user_data_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_data_user_id_seq', 15, true);


--
-- TOC entry 2871 (class 2606 OID 16427)
-- Name: cards cards_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT cards_pkey PRIMARY KEY (card_id);


--
-- TOC entry 2873 (class 2606 OID 16421)
-- Name: categories categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (cat_id);


--
-- TOC entry 2875 (class 2606 OID 16451)
-- Name: elements elements_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.elements
    ADD CONSTRAINT elements_name_key UNIQUE (name);


--
-- TOC entry 2877 (class 2606 OID 16449)
-- Name: elements elements_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.elements
    ADD CONSTRAINT elements_pkey PRIMARY KEY (element_id);


--
-- TOC entry 2867 (class 2606 OID 16403)
-- Name: users user_data_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT user_data_pkey PRIMARY KEY (user_id);


--
-- TOC entry 2869 (class 2606 OID 16405)
-- Name: users user_data_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT user_data_username_key UNIQUE (username);


--
-- TOC entry 2878 (class 2606 OID 16432)
-- Name: cards categorie_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT categorie_relation FOREIGN KEY (cat_id) REFERENCES public.categories(cat_id);


--
-- TOC entry 2880 (class 2606 OID 16452)
-- Name: cards element_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT element_relation FOREIGN KEY (element_id) REFERENCES public.elements(element_id);


--
-- TOC entry 2879 (class 2606 OID 16437)
-- Name: cards user_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT user_relation FOREIGN KEY (user_id) REFERENCES public.users(user_id);


-- Completed on 2021-01-04 19:42:12

--
-- PostgreSQL database dump complete
--

