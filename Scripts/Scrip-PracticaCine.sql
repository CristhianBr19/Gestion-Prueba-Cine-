--
-- PostgreSQL database dump
--

\restrict arNeCDPkRB7I5jW50OOBhxCv0uPXVDaKgEA4GvfCf8489qE3StUzUIPKlOfjnP8

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-04-17 13:53:37

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 225 (class 1255 OID 16447)
-- Name: fn_consultar_disponibilidad_sala(character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.fn_consultar_disponibilidad_sala(p_nombre_sala character varying) RETURNS text
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_conteo INTEGER;
    v_mensaje TEXT;
BEGIN
    -- 1. Contar películas activas vinculadas a la sala
    SELECT COUNT(*)
    INTO v_conteo
    FROM pelicula_salacine ps
    JOIN sala_cine s ON ps.id_sala = s.id
    WHERE s.nombre = p_nombre_sala 
      AND ps.active = true 
      AND s.active = true;

    -- 2. Lógica de decisión mejorada
    IF v_conteo > 5 THEN
        v_mensaje := 'Sala no disponible'; 
    ELSIF v_conteo >= 3 THEN
        v_mensaje := 'Sala con ' || v_conteo || ' películas asignadas'; 
    ELSE
        v_mensaje := 'Sala disponible'; 
    END IF;

    RETURN v_mensaje;
END;
$$;


ALTER FUNCTION public.fn_consultar_disponibilidad_sala(p_nombre_sala character varying) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 220 (class 1259 OID 16403)
-- Name: pelicula; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pelicula (
    id integer NOT NULL,
    nombre character varying NOT NULL,
    duracion integer NOT NULL,
    active boolean NOT NULL,
    fecha_publicacion date
);


ALTER TABLE public.pelicula OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16402)
-- Name: pelicula_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.pelicula_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.pelicula_id_seq OWNER TO postgres;

--
-- TOC entry 5038 (class 0 OID 0)
-- Dependencies: 219
-- Name: pelicula_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.pelicula_id_seq OWNED BY public.pelicula.id;


--
-- TOC entry 224 (class 1259 OID 16427)
-- Name: pelicula_salacine; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pelicula_salacine (
    id integer NOT NULL,
    id_pelicula integer,
    id_sala integer,
    fecha_publicacion date NOT NULL,
    fecha_fin date,
    active boolean DEFAULT true
);


ALTER TABLE public.pelicula_salacine OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16426)
-- Name: pelicula_salacine_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.pelicula_salacine_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.pelicula_salacine_id_seq OWNER TO postgres;

--
-- TOC entry 5039 (class 0 OID 0)
-- Dependencies: 223
-- Name: pelicula_salacine_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.pelicula_salacine_id_seq OWNED BY public.pelicula_salacine.id;


--
-- TOC entry 222 (class 1259 OID 16416)
-- Name: sala_cine; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sala_cine (
    id integer NOT NULL,
    nombre character varying(100) NOT NULL,
    estado character varying(50) NOT NULL,
    active boolean DEFAULT true
);


ALTER TABLE public.sala_cine OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16415)
-- Name: sala_cine_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sala_cine_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sala_cine_id_seq OWNER TO postgres;

--
-- TOC entry 5040 (class 0 OID 0)
-- Dependencies: 221
-- Name: sala_cine_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sala_cine_id_seq OWNED BY public.sala_cine.id;


--
-- TOC entry 4867 (class 2604 OID 16406)
-- Name: pelicula id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula ALTER COLUMN id SET DEFAULT nextval('public.pelicula_id_seq'::regclass);


--
-- TOC entry 4870 (class 2604 OID 16430)
-- Name: pelicula_salacine id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine ALTER COLUMN id SET DEFAULT nextval('public.pelicula_salacine_id_seq'::regclass);


--
-- TOC entry 4868 (class 2604 OID 16419)
-- Name: sala_cine id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sala_cine ALTER COLUMN id SET DEFAULT nextval('public.sala_cine_id_seq'::regclass);


--
-- TOC entry 5028 (class 0 OID 16403)
-- Dependencies: 220
-- Data for Name: pelicula; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pelicula (id, nombre, duracion, active, fecha_publicacion) FROM stdin;
8	Tortugas Ninja	122	t	2024-04-01
7	Transformers	120	t	2026-03-22
9	Cars 3	140	t	2026-04-01
10	Deadpool	123	f	2026-03-12
12	Spider-Man	600	t	2026-04-08
13	El Origen	200	t	2026-03-06
14	Cars 2	120	t	2026-04-02
\.


--
-- TOC entry 5032 (class 0 OID 16427)
-- Dependencies: 224
-- Data for Name: pelicula_salacine; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pelicula_salacine (id, id_pelicula, id_sala, fecha_publicacion, fecha_fin, active) FROM stdin;
5	7	2	2026-04-04	2026-04-07	t
7	10	1	2026-03-12	2026-04-04	t
8	8	1	2026-03-04	2026-04-06	t
10	12	1	2026-04-01	2026-04-16	t
11	9	1	2026-04-02	2026-04-15	t
12	13	1	2026-03-13	2026-04-01	t
\.


--
-- TOC entry 5030 (class 0 OID 16416)
-- Dependencies: 222
-- Data for Name: sala_cine; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.sala_cine (id, nombre, estado, active) FROM stdin;
4	Sala VIP 03	Operativa	t
3	Sala VIP 03	Operativa	t
2	Sala VIP 02	Mantenimiento	t
5	Sala IMAX 02	Operativa	t
1	Sala IMAX 01	Mantenimiento	t
\.


--
-- TOC entry 5041 (class 0 OID 0)
-- Dependencies: 219
-- Name: pelicula_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pelicula_id_seq', 14, true);


--
-- TOC entry 5042 (class 0 OID 0)
-- Dependencies: 223
-- Name: pelicula_salacine_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pelicula_salacine_id_seq', 12, true);


--
-- TOC entry 5043 (class 0 OID 0)
-- Dependencies: 221
-- Name: sala_cine_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.sala_cine_id_seq', 5, true);


--
-- TOC entry 4873 (class 2606 OID 16414)
-- Name: pelicula pelicula_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula
    ADD CONSTRAINT pelicula_pkey PRIMARY KEY (id);


--
-- TOC entry 4877 (class 2606 OID 16435)
-- Name: pelicula_salacine pelicula_salacine_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine
    ADD CONSTRAINT pelicula_salacine_pkey PRIMARY KEY (id);


--
-- TOC entry 4875 (class 2606 OID 16425)
-- Name: sala_cine sala_cine_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sala_cine
    ADD CONSTRAINT sala_cine_pkey PRIMARY KEY (id);


--
-- TOC entry 4878 (class 2606 OID 16436)
-- Name: pelicula_salacine pelicula_salacine_id_pelicula_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine
    ADD CONSTRAINT pelicula_salacine_id_pelicula_fkey FOREIGN KEY (id_pelicula) REFERENCES public.pelicula(id);


--
-- TOC entry 4879 (class 2606 OID 16441)
-- Name: pelicula_salacine pelicula_salacine_id_sala_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pelicula_salacine
    ADD CONSTRAINT pelicula_salacine_id_sala_fkey FOREIGN KEY (id_sala) REFERENCES public.sala_cine(id);


-- Completed on 2026-04-17 13:53:38

--
-- PostgreSQL database dump complete
--

\unrestrict arNeCDPkRB7I5jW50OOBhxCv0uPXVDaKgEA4GvfCf8489qE3StUzUIPKlOfjnP8

