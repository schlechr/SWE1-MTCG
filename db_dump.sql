--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

-- Started on 2021-01-10 15:20:02

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
-- TOC entry 200 (class 1259 OID 16406)
-- Name: cards; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cards (
    card_id character varying(50) NOT NULL,
    name character varying(50) NOT NULL,
    damage double precision NOT NULL,
    cat_id integer NOT NULL,
    username character varying(50),
    package_id integer NOT NULL,
    element_id integer NOT NULL,
    mt_id integer
);


ALTER TABLE public.cards OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 16414)
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categories (
    cat_id integer NOT NULL,
    name character varying(50) NOT NULL
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- TOC entry 206 (class 1259 OID 16491)
-- Name: decks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.decks (
    username character varying(50) NOT NULL,
    card1 character varying(50),
    card2 character varying(50),
    card3 character varying(50),
    card4 character varying(50),
    fight_lock boolean
);


ALTER TABLE public.decks OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 16444)
-- Name: elements; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.elements (
    element_id integer NOT NULL,
    name character varying(50) NOT NULL,
    effectiv character varying(50)
);


ALTER TABLE public.elements OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 16442)
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
-- TOC entry 3050 (class 0 OID 0)
-- Dependencies: 202
-- Name: elements_element_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.elements_element_id_seq OWNED BY public.elements.element_id;


--
-- TOC entry 204 (class 1259 OID 16467)
-- Name: monstertypes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.monstertypes (
    mt_id integer NOT NULL,
    name character varying(50)
);


ALTER TABLE public.monstertypes OWNER TO postgres;

--
-- TOC entry 207 (class 1259 OID 16521)
-- Name: scoreboard; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.scoreboard (
    username character varying(50) NOT NULL,
    wins integer,
    draws integer,
    loses integer
);


ALTER TABLE public.scoreboard OWNER TO postgres;

--
-- TOC entry 208 (class 1259 OID 16526)
-- Name: trades; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.trades (
    trade_id character varying(50) NOT NULL,
    card_to_trade character varying(50) NOT NULL,
    req_type character varying(50) NOT NULL,
    req_min_dmg integer NOT NULL
);


ALTER TABLE public.trades OWNER TO postgres;

--
-- TOC entry 205 (class 1259 OID 16481)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    username character varying(50) NOT NULL,
    password character varying(50) NOT NULL,
    coins integer,
    active boolean,
    name character varying(50),
    bio character varying(50),
    image character varying(50)
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 2878 (class 2604 OID 16447)
-- Name: elements element_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.elements ALTER COLUMN element_id SET DEFAULT nextval('public.elements_element_id_seq'::regclass);


--
-- TOC entry 3036 (class 0 OID 16406)
-- Dependencies: 200
-- Data for Name: cards; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.cards (card_id, name, damage, cat_id, username, package_id, element_id, mt_id) FROM stdin;
845f0dc7-37d0-426e-994e-43fc3ac83c08	WaterGoblin	10	1	kienboec	1	2	2
99f8f8dc-e25e-4a95-aa2c-782823f36e2a	Dragon	50	1	kienboec	1	3	3
e85e3976-7c86-4d06-9a80-641c2019a79f	WaterSpell	20	2	kienboec	1	2	1
dfdd758f-649c-40f9-ba3a-8657f4b3439f	FireSpell	25	2	kienboec	1	3	1
644808c2-f87a-4600-b313-122b02322fd5	WaterGoblin	9	1	kienboec	2	2	2
4a2757d6-b1c3-47ac-b9a3-91deab093531	Dragon	55	1	kienboec	2	3	3
91a6471b-1426-43f6-ad65-6fc473e16f9f	WaterSpell	21	2	kienboec	2	2	1
4ec8b269-0dfa-4f97-809a-2c63fe2a0025	Ork	55	1	kienboec	2	1	5
f8043c23-1534-4487-b66b-238e0c3c39b5	WaterSpell	23	2	kienboec	2	2	1
b017ee50-1c14-44e2-bfd6-2c0c5653a37c	WaterGoblin	11	1	kienboec	3	2	2
d04b736a-e874-4137-b191-638e0ff3b4e7	Dragon	70	1	kienboec	3	3	3
88221cfe-1f84-41b9-8152-8e36c6a354de	WaterSpell	22	2	kienboec	3	2	1
1d3f175b-c067-4359-989d-96562bfa382c	Ork	40	1	kienboec	3	1	5
171f6076-4eb5-4a7d-b3f2-2d650cc3d237	RegularSpell	28	2	kienboec	3	1	1
ed1dc1bc-f0aa-4a0c-8d43-1402189b33c8	WaterGoblin	10	1	kienboec	4	2	2
65ff5f23-1e70-4b79-b3bd-f6eb679dd3b5	Dragon	50	1	kienboec	4	3	3
55ef46c4-016c-4168-bc43-6b9b1e86414f	WaterSpell	20	2	kienboec	4	2	1
f3fad0f2-a1af-45df-b80d-2e48825773d9	Ork	45	1	kienboec	4	1	5
8c20639d-6400-4534-bd0f-ae563f11f57a	WaterSpell	25	2	kienboec	4	2	1
d7d0cb94-2cbf-4f97-8ccf-9933dc5354b8	WaterGoblin	9	1	altenhof	5	2	2
44c82fbc-ef6d-44ab-8c7a-9fb19a0e7c6e	Dragon	55	1	altenhof	5	3	3
2c98cd06-518b-464c-b911-8d787216cddd	WaterSpell	21	2	altenhof	5	2	1
dcd93250-25a7-4dca-85da-cad2789f7198	FireSpell	23	2	altenhof	5	3	1
b2237eca-0271-43bd-87f6-b22f70d42ca4	WaterGoblin	11	1	altenhof	6	2	2
9e8238a4-8a7a-487f-9f7d-a8c97899eb48	Dragon	70	1	altenhof	6	3	3
d60e23cf-2238-4d49-844f-c7589ee5342e	WaterSpell	22	2	altenhof	6	2	1
fc305a7a-36f7-4d30-ad27-462ca0445649	Ork	40	1	altenhof	6	1	5
84d276ee-21ec-4171-a509-c1b88162831c	RegularSpell	28	2	altenhof	6	1	1
2272ba48-6662-404d-a9a1-41a9bed316d9	WaterGoblin	11	1	admin	9	2	2
3871d45b-b630-4a0d-8bc6-a5fc56b6a043	Dragon	70	1	admin	9	3	3
166c1fd5-4dcb-41a8-91cb-f45dcd57cef3	Knight	22	1	admin	9	1	6
237dbaef-49e3-4c23-b64b-abf5c087b276	WaterSpell	40	2	admin	9	2	1
27051a20-8580-43ff-a473-e986b52f297a	FireElf	28	1	admin	9	3	8
67f9048f-99b8-4ae4-b866-d8008d00c53d	WaterGoblin	10	1	altenhof	7	2	2
aa9999a0-734c-49c6-8f4a-651864b14e62	RegularSpell	50	2	altenhof	7	1	1
d6e9c720-9b5a-40c7-a6b2-bc34752e3463	Knight	20	1	altenhof	7	1	6
02a9c76e-b17d-427f-9240-2dd49b0d3bfd	RegularSpell	45	2	altenhof	7	1	1
2508bf5c-20d7-43b4-8c77-bc677decadef	FireElf	25	1	altenhof	7	3	8
70962948-2bf7-44a9-9ded-8c68eeac7793	WaterGoblin	9	1	altenhof	8	2	2
74635fae-8ad3-4295-9139-320ab89c2844	FireSpell	55	2	altenhof	8	3	1
ce6bcaee-47e1-4011-a49e-5a4d7d4245f3	Knight	21	1	altenhof	8	1	6
a6fde738-c65a-4b10-b400-6fef0fdb28ba	FireSpell	55	2	altenhof	8	3	1
a1618f1e-4f4c-4e09-9647-87e16f1edd2d	FireElf	23	1	altenhof	8	3	8
1cb6ab86-bdb2-47e5-b6e4-68c5ab389334	Ork	45	1	altenhof	1	1	5
951e886a-0fbf-425d-8df5-af2ee4830d85	Ork	55	1	kienboec	5	1	5
\.


--
-- TOC entry 3037 (class 0 OID 16414)
-- Dependencies: 201
-- Data for Name: categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.categories (cat_id, name) FROM stdin;
1	monster card
2	spell cards
\.


--
-- TOC entry 3042 (class 0 OID 16491)
-- Dependencies: 206
-- Data for Name: decks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.decks (username, card1, card2, card3, card4, fight_lock) FROM stdin;
altenhof	aa9999a0-734c-49c6-8f4a-651864b14e62	d6e9c720-9b5a-40c7-a6b2-bc34752e3463	d60e23cf-2238-4d49-844f-c7589ee5342e	02a9c76e-b17d-427f-9240-2dd49b0d3bfd	f
\.


--
-- TOC entry 3039 (class 0 OID 16444)
-- Dependencies: 203
-- Data for Name: elements; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.elements (element_id, name, effectiv) FROM stdin;
3	Fire	Normal
1	Normal	Water
2	Water	Fire
\.


--
-- TOC entry 3040 (class 0 OID 16467)
-- Dependencies: 204
-- Data for Name: monstertypes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.monstertypes (mt_id, name) FROM stdin;
1	Spell
2	Goblin
3	Dragon
4	Wizzard
5	Ork
6	Knight
7	Kraken
8	Elf
\.


--
-- TOC entry 3043 (class 0 OID 16521)
-- Dependencies: 207
-- Data for Name: scoreboard; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.scoreboard (username, wins, draws, loses) FROM stdin;
	0	0	0
altenhof	10	0	5
kienboec	5	0	10
\.


--
-- TOC entry 3044 (class 0 OID 16526)
-- Dependencies: 208
-- Data for Name: trades; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.trades (trade_id, card_to_trade, req_type, req_min_dmg) FROM stdin;
\.


--
-- TOC entry 3041 (class 0 OID 16481)
-- Dependencies: 205
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (username, password, coins, active, name, bio, image) FROM stdin;
admin	istrator	20	t	\N	\N	\N
kienboec	daniel	0	t	Kienboeck	me playin...	:-)
altenhof	markus	0	t	Altenhofer	me codin...	:-D
\.


--
-- TOC entry 3051 (class 0 OID 0)
-- Dependencies: 202
-- Name: elements_element_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.elements_element_id_seq', 1, false);


--
-- TOC entry 2880 (class 2606 OID 16427)
-- Name: cards cards_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT cards_pkey PRIMARY KEY (card_id);


--
-- TOC entry 2882 (class 2606 OID 16421)
-- Name: categories categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (cat_id);


--
-- TOC entry 2892 (class 2606 OID 16495)
-- Name: decks decks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.decks
    ADD CONSTRAINT decks_pkey PRIMARY KEY (username);


--
-- TOC entry 2884 (class 2606 OID 16451)
-- Name: elements elements_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.elements
    ADD CONSTRAINT elements_name_key UNIQUE (name);


--
-- TOC entry 2886 (class 2606 OID 16449)
-- Name: elements elements_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.elements
    ADD CONSTRAINT elements_pkey PRIMARY KEY (element_id);


--
-- TOC entry 2888 (class 2606 OID 16471)
-- Name: monstertypes monstertypes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.monstertypes
    ADD CONSTRAINT monstertypes_pkey PRIMARY KEY (mt_id);


--
-- TOC entry 2894 (class 2606 OID 16525)
-- Name: scoreboard scoreboard_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.scoreboard
    ADD CONSTRAINT scoreboard_pkey PRIMARY KEY (username);


--
-- TOC entry 2896 (class 2606 OID 16530)
-- Name: trades trades_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trades
    ADD CONSTRAINT trades_pkey PRIMARY KEY (trade_id);


--
-- TOC entry 2890 (class 2606 OID 16485)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (username);


--
-- TOC entry 2897 (class 2606 OID 16432)
-- Name: cards categorie_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT categorie_relation FOREIGN KEY (cat_id) REFERENCES public.categories(cat_id);


--
-- TOC entry 2902 (class 2606 OID 16501)
-- Name: decks decks_card1_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.decks
    ADD CONSTRAINT decks_card1_relation FOREIGN KEY (card1) REFERENCES public.cards(card_id);


--
-- TOC entry 2903 (class 2606 OID 16506)
-- Name: decks decks_card2_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.decks
    ADD CONSTRAINT decks_card2_relation FOREIGN KEY (card2) REFERENCES public.cards(card_id);


--
-- TOC entry 2904 (class 2606 OID 16511)
-- Name: decks decks_card3_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.decks
    ADD CONSTRAINT decks_card3_relation FOREIGN KEY (card3) REFERENCES public.cards(card_id);


--
-- TOC entry 2905 (class 2606 OID 16516)
-- Name: decks decks_card4_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.decks
    ADD CONSTRAINT decks_card4_relation FOREIGN KEY (card4) REFERENCES public.cards(card_id);


--
-- TOC entry 2901 (class 2606 OID 16496)
-- Name: decks decks_user_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.decks
    ADD CONSTRAINT decks_user_relation FOREIGN KEY (username) REFERENCES public.users(username);


--
-- TOC entry 2898 (class 2606 OID 16452)
-- Name: cards element_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT element_relation FOREIGN KEY (element_id) REFERENCES public.elements(element_id);


--
-- TOC entry 2899 (class 2606 OID 16472)
-- Name: cards monstertype_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT monstertype_relation FOREIGN KEY (mt_id) REFERENCES public.monstertypes(mt_id);


--
-- TOC entry 2900 (class 2606 OID 16486)
-- Name: cards user_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cards
    ADD CONSTRAINT user_relation FOREIGN KEY (username) REFERENCES public.users(username);


-- Completed on 2021-01-10 15:20:03

--
-- PostgreSQL database dump complete
--

